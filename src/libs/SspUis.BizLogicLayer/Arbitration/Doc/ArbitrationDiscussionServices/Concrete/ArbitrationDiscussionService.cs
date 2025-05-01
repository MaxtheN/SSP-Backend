using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.Arbitration.Doc.ArbitrationDiscussionServices.QueryObjects;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer;

public class ArbitrationDiscussionService : BaseEntityService<long,
    ArbitrationDiscussion, ArbitrationDiscussionListDto,
    ArbitrationDiscussionDto, CreateArbitrationDiscussionDlDto,
    UpdateArbitrationDiscussionDlDto, IArbitrationDiscussionRepository,
    ArbitrationDiscussionSortFilterOptions>,
    IArbitrationDiscussionService
{

    #region ctor
    private readonly INumberService _numberService;
    private readonly IEImzoService _eImzoService;
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IConvertService _pdfConvert;
    private readonly IArbitrationCourtApplicationService _arbitrationCourtApplicationService;

    public ArbitrationDiscussionService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IEImzoService eImzoService,
        IStorageService storageService,
        IDocumentChangeLogService documentChangeLogService,
        IConvertService convert,
        IArbitrationCourtApplicationService arbitrationCourtApplicationService,
        IAuthService authService) : base(unitOfWork)
    {
        this._numberService = numberService;
        this._eImzoService = eImzoService;
        this._authService = authService;
        this._storageService = storageService;
        this._documentChangeLogService = documentChangeLogService;
        this._pdfConvert = convert;
        this._arbitrationCourtApplicationService = arbitrationCourtApplicationService;
    }

    #endregion

    public override ArbitrationDiscussionDto Get(long id)
    {

        var entity = UnitOfWork.Context.Set<ArbitrationDiscussion>()
            .Include(x => x.Signs)
            .Include(x => x.Files)
            .FirstOrDefault(x => x.Id == id);

        var dto = Repository.ById<ArbitrationDiscussionDto>(id);
        if (dto == null)
        {
            AddError("not founded");
        }
        dto.CanSign = StatusIdConst
               .CanArbitrationDiscussionApplyStatus(dto.StatusId,
               StatusIdConst.SIGNED);
        dto.CanDelete = StatusIdConst
               .CanArbitrationDiscussionApplyStatus(dto.StatusId,
               StatusIdConst.DELETED);
        dto.CanEdit = StatusIdConst
               .CanArbitrationDiscussionApplyStatus(dto.StatusId,
               StatusIdConst.MODIFIED);

        foreach (var file in dto.Files)
        {
            if (file.FileExtension == ".docx" || file.FileExtension == ".doc")
            {
                file.FileExtension = ".pdf";
                file.FileName = file.FileName.Replace(".docx", ".pdf");
                file.FileName = file.FileName.Replace(".doc", ".pdf");
            }
        }
        return dto;
    }

    public override PagedResult<ArbitrationDiscussionListDto> GetList(ArbitrationDiscussionSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<ArbitrationDiscussionListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

    public async Task<ArbitrationDiscussionDto> GetByArbitrationCourtApplicationId(int arbitrationCourtApplicationId)
    {
        //var application = UnitOfWork.ArbitrationCourtApplicationRepository.AllAsQueryable
        //	.Include(x=>x.Application)
        //   .FirstOrDefault(x => x.Id == arbitrationCourtApplicationId);

        var application = UnitOfWork
            .ArbitrationCourtApplicationRepository
            .ById<ArbitrationCourtApplicationDto>(arbitrationCourtApplicationId);

        try
        {
            var result = new ArbitrationDiscussionDto()
            {
                DocOn = DateTime.Now.AsDateOnly(),
                DocNumber = _numberService.GetNext(
                            NumberTemplateDocumentConst.DOC_ARBITRATION_DISCUSSION,
                            _authService.User.OrganizationId).Item2,
                ArbitrationCourtApplicationId = arbitrationCourtApplicationId,
                ContractorId = application.Application.ContractorId,
                ResponsibleContractorId = application.ResponsibleContractorId,
                ResponsibleContractorInnPinfl = application.ResponsibleInnPnfl,
                ContractorName = application.Application.Contractor,
                ResponsibleContractorName = application.Responsible,
                Status = application.Application.Status,
                ArbitrationStep = application.Application.CurrentStep.FullName,
                DiscussionDate = DateTime.Now
            };
            return result;
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }
    public override HaveId<long> Create(CreateArbitrationDiscussionDlDto dto)
    {

        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }
                UnitOfWork.Save();
                _storageService.MoveToPersistent(
                        DocumentStorageConst.DOC_ARBITRATION_DISCUSSION
                        , entity.Id.ToString()
                        , dto.Files.Select(x => x.Id).ToArray()
                    );

                var discussion = UnitOfWork.ArbitrationDiscussionRepository.AllAsQueryable
                    .Include(x => x.ArbitrationCourtApplication)
                         .FirstOrDefault(x => x.Id == entity.Id);
                var application = UnitOfWork.ArbitrationCourtApplicationRepository.AllAsQueryable
                    .Include(x => x.Signer)
                     .FirstOrDefault(x => x.Id == discussion.ArbitrationCourtApplicationId);

                if (application.Signer == null)
                {
                    AddError("Signer empty");
                    return null;
                }
                ArbitrationDiscussionSign discussionSigner = null;
                //var applicationSigners = application.Signer.ToList();
                //foreach (var item in applicationSigners)
                //{
                //    if (item.ArbitrationJudgeId != null)
                //    {
                //        discussionSigner = new ArbitrationDiscussionSign
                //        {
                //            ArbitrationJudgeId = item.ArbitrationJudgeId.Value,
                //            //SignFile = item.SignFile.Value,
                //            //DataFile = item.DataFile.Value,
                //            //CreatedAt = DateTime.UtcNow,
                //            //SignAt = item.SignedAt
                //        };
                //        discussion.Signs.Add(discussionSigner);
                //    }
                //}
                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch (Exception ex)
            {
                AddError($"Error: {ex.Message}, InnerException: {ex.InnerException}");
                transaction.Rollback();
                return null;
            }
        }
    }
    public async Task<HaveId<long>> Update(UpdateArbitrationDiscussionDlDto dto)
    {
        var transaction = await UnitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            var entity = Repository.Update(dto, x =>
            {
                if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(x.StatusId, StatusIdConst.MODIFIED))
                    Repository.AddError("Имкони йўқ / Нет доступа");
            });
            CombineStatuses(Repository);
            if (HasErrors)
            {
                await transaction.RollbackAsync();
                return null;
            }
            UnitOfWork.Save();
            if (IsValid)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            AddError(ex.Message + " Inner: " + ex.InnerException);
            return null;
        }
    }
    public async Task<HaveId<long>> Sign(SignStatusArbitrationDiscussionDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ??
            await UnitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            var entity = Repository.AllAsQueryable
                   .Include(x => x.Signs)
                   .FirstOrDefault(x => x.Id == dto.Id);

            if (entity == null)
            {
                AddError("Not found");
                transaction.Rollback();
                return null;
            }
            var eImzoTimstampDto = new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = null,
                Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
            };

            var app = UnitOfWork.ArbitrationCourtApplicationRepository.AllAsQueryable
                    .Include(x => x.Signer)
                    .ThenInclude(x => x.ArbitrationJudge)
                        .FirstOrDefault(x => x.Id == entity.ArbitrationCourtApplicationId);
            if (app == null)
            {
                AddError("Not fount Application");
                return null;
            }

            //if (!app.Signer
            //        .Any(x => x.ArbitrationJudge.Person.Pinfl == eImzoTimstampDto.Pinfl))
            //{
            //    AddError("Нет доступ");
            //}
            var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

            CombineStatuses(_eImzoService);
            if (HasErrors)
            {
                await transaction.RollbackAsync();
                return null;
            };


            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
            });
            //if (app.Signer is not null)
            //{
            //    var appSiner = app.Signer.ToList();
            //    for (int i = 0; i < appSiner.Count; i++)
            //    {
            //var arbitrationJudge = UnitOfWork.ArbitrationJudgeRepository.AllAsQueryable
            //    .FirstOrDefault(x => x.Person.Pinfl == _authService.User.Pinfl);

            var signer = new ArbitrationDiscussionSign()
            {
                SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt"),
                DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt"),
                SignedAt = DateTime.Now,
                SignedUserInfo = _authService.User != null
                ? _authService.User.ToTextForDocumentLog()
                : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn,
                StatusId = dto.StatusId,
                //ArbitrationJudgeId = arbitrationJudge.Id
            };
            entity.Signs.Add(signer);
            UnitOfWork.Save();
            //    }
            //}
            //else
            //{
            //    AddError("Signer not found in ArbitrationCourtApplication");
            //    return null;
            //}
            var ent = Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, StatusIdConst.SIGNED))
                    Repository.AddError("Нет доступа");
            });
            CombineStatuses(Repository);
            if (HasErrors)
                return null;

            _arbitrationCourtApplicationService.DiscussionStep(new()
            {
                Id = ent.ArbitrationCourtApplicationId
            });

            CombineStatuses(_arbitrationCourtApplicationService);
            if (HasErrors)
            {
                await transaction.RollbackAsync();
                return null;
            }
            //var arbitration = _arbitrationCourtApplicationService.StepFromSecondToThird(app);

            await UnitOfWork.Context.SaveChangesAsync();

            if (IsValid && canCommit)
                await transaction.CommitAsync();
            return HaveId.Create(entity.Id);
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                await transaction.RollbackAsync();
        }
        return null;
    }
    public override void Delete(long id)

    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusArbitrationDiscussionDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
                });
                Repository.UpdateStatus(statusDto);
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteArbitrationDiscussion");

                if (IsValid)
                {
                    transaction.Commit();
                }
            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusArbitrationDiscussionDlDto dto, Action<ArbitrationDiscussion> validation)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        using var transaction = canCommit ? UnitOfWork.BeginTransaction() : null;
        validation += Validation(dto);
        try
        {
            var entity = Repository.UpdateStatus(dto, validation);
            CombineStatuses(Repository);
            if (HasErrors)
                return null;
            UnitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "ArbitrationDiscussion");
            if (IsValid && canCommit)
                transaction.Commit();
            return res;
        }
       finally
        {
            transaction?.Dispose();
        }
    }
    private Action<ArbitrationDiscussion> Validation(UpdateStatusArbitrationDiscussionDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<ArbitrationDiscussionDto>(id, applyFilter: false);
        _documentChangeLogService.Create<long, ArbitrationDiscussionDto>(
            dto: moveDto,
            tableId: TableIdConst.ARBITRATION_DISCUSSION,
            organizationId: _authService.User.OrganizationId,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    #region Files Downloands
    public async Task<byte[]> DownloadTemplate()
    {
        try
        {
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName("ru",
                StaticFileConst.WordTemplate.DOWNLOAD_TEMP)
                );

            return wordFile.ToArray();
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
        return null;
    }
    private async Task<byte[]> DownloadPdf(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        var storageFile = Download(fileId, entity, storageDocument);
        var stream = storageFile.GetStream();
        MemoryStream wordFile = new();
        await wordFile.WriteAsync(stream.ReadAsBytes());
        var doc = Repository.ReadAsNoTracked<ArbitrationDiscussionDto>(x => x.Files.Any(f => f.Id == fileId))
            .FirstOrDefault();
        if (doc == null)
        {
            AddError("Hujjat topilmadi.");
            return null;
        }
        var data = Repository.ById<ArbitrationDiscussionDto>(doc.Id);
        var plh = WordFactory.MakePlaceholders(data);
        #region QrCodes

        //var signSsp = UnitOfWork.Context
        //  .Set<ArbitrationCourtApplicationSigner>()
        //  .FirstOrDefault(s => s.OwnerId == doc.Id);
        //var signContractor = UnitOfWork.Context
        //    .Set<AdditionalAgreementSign>()
        //.FirstOrDefault(s =>
        //s.OwnerId == doc.Id
        //    && s.StatusId == StatusIdConst.SIGNED);


        //var link = _systemConf.QrImagePrintMy + "/Arbitration/ArbitrationCourtApplication/DownloadPdf?fileId=" + storageFile.FileId.ToString();

        //var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        //plh.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });

        //if (signSsp != null)
        //{
        //    var QrCodeSsp = new MemoryStream(QRCodeHelper.GeneratePng(doc.Id.ToString()
        //            + "  " + signSsp.SignedUserInfo));
        //    plh.ImagePlaceholders.Add("QrCodeSsp",
        //        new() { Dpi = 512, MemStream = QrCodeSsp });
        //    plh.TextPlaceholders.Add("QrCodeSsp", "++QrCodeSsp++");
        //}
        //else
        //    plh.TextPlaceholders.Add("QrCodeSsp", "");
        //if (signContractor != null)
        //{
        //    var QrCodeContractor = new MemoryStream(QRCodeHelper.GeneratePng(doc.Id.ToString()
        //           + "  " + signContractor.SignedUserInfo));

        //    plh.ImagePlaceholders.Add("QrCode",
        //        new() { Dpi = 512, MemStream = QrCodeContractor });
        //    plh.TextPlaceholders.Add("QrCode", "++QrCode++");
        //}
        //else
        //    plh.TextPlaceholders.Add("QrCode", "");
        #endregion

        wordFile = (new DocXHandler((MemoryStream)wordFile, plh)).ReplaceAll();
        var res = await _pdfConvert.DocxToPdfAsync((MemoryStream)wordFile, new());

        return res;
    }
    public object UploadFiles(params StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("Empty file");
            return null;
        }
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_ARBITRATION_DISCUSSION, files)
            .Select(a => new ArbitrationDiscussionFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                FileExtension = Path.GetExtension(a.FileName),
                CreatedAt = DateTime.Now
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save(
            $"{nameof(TableIdConst.ARBITRATION_DISCUSSION)}_SIGN_DATA",
            docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    private HaveId<long> CreateDocumentChangeLog(
        long id,
        string message = null,
        string userIp = null,
        string userAgent = null)
    {
        var entityDto = Repository.ById<ArbitrationDiscussionDto>(
            id, applyFilter: false);
        _documentChangeLogService.Create<long, ArbitrationDiscussionDto>(
            dto: entityDto,
            tableId: TableIdConst.ARBITRATION_DISCUSSION,
            organizationId: _authService.Organization.Id,
            statusId: entityDto.StatusId,
            userIp: userIp,
            userAgent: userAgent,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = UnitOfWork.Context.Set<ArbitrationDiscussionFile>().FirstOrDefault(a => a.Id == fileId);
        StorageFile file;
        if (entity == null)
            AddError("file not found");

        file = new(
        fileId: fileId,
        fileName: entity.FileName.Replace(entity.FileExtension, ".pdf"),
        stream: new MemoryStream(DownloadPdf(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_DISCUSSION).Result));
        //}
        //else
        //file = Download(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_DISCUSSION);

        return file;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = UnitOfWork.Context
            .Set<ArbitrationDiscussionFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_DISCUSSION);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }
    private StorageFile Download(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        StorageFile file;

        if (entity == null)
        {
            file = _storageService.GetTempFile(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
        else
        {
            file = _storageService.GetFile(storageDocument, entity.OwnerId.ToString(), fileId);
            CombineStatuses(_storageService);

            if (IsValid)
                file.FileName = entity.FileName;
        }

        return file;
    }
    #endregion
}
