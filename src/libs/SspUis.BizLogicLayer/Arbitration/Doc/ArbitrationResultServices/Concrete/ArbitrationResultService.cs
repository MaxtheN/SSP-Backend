using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

public class ArbitrationResultService : BaseEntityService<long,
    ArbitrationResult, ArbitrationResultListDto,
    ArbitrationResultDto, CreateArbitrationResultDlDto,
    UpdateArbitrationResultDlDto, IArbitrationResultRepository,
    ArbitrationResultSortFilterOptions>,
    IArbitrationResultService
{

    #region ctor
    private readonly INumberService _numberService;
    private readonly IEImzoService _eImzoService;
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IConvertService _pdfConvert;
    private readonly IArbitrationCourtApplicationService _arbitrationCourtApplicationService;

    public ArbitrationResultService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IEImzoService eImzoService,
        IStorageService storageService,
        IDocumentChangeLogService documentChangeLogService,
        IConvertService convert,
        IAuthService authService,
        IArbitrationCourtApplicationService arbitrationCourtApplicationService) : base(unitOfWork)
    {
        _numberService = numberService;
        _eImzoService = eImzoService;
        _authService = authService;
        _storageService = storageService;
        _documentChangeLogService = documentChangeLogService;
        _pdfConvert = convert;
        _arbitrationCourtApplicationService = arbitrationCourtApplicationService;
    }
    #endregion

    public async Task<ArbitrationResultDto> GetByArbitrationCourtApplicationId(long arbitrationCourtApplicationId)
    {
        var application = UnitOfWork.ArbitrationCourtApplicationRepository
            .ById<ArbitrationCourtApplicationDto>(arbitrationCourtApplicationId);

        var result = new ArbitrationResultDto()
        {
            DocOn = DateTime.Now.AsDateOnly(),
            DocNumber = _numberService.GetNext(
                NumberTemplateDocumentConst.DOC_ARBITRATION_RESULT,
                _authService.User.OrganizationId).Item2,
            ArbitrationCourtApplicationId = arbitrationCourtApplicationId,
            ArbitrationCourtApplication = application,
            ContractorId = application.Application.ContractorId,
            Contractor = application.Application.Contractor,
            ResponsibleContractorId = application.ResponsibleContractorId,
            ResponsibleContractor = application.Responsible,
            Amount = application.Amount
        };
        return result;
    }
    public override HaveId<long> Create(CreateArbitrationResultDlDto dto)
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
                        DocumentStorageConst.DOC_ARBITRATION_RESULT
                        , entity.Id.ToString()
                        , dto.Files.Select(x => x.Id).ToArray()
                    );
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

    public override ArbitrationResultDto Get(long id)
    {
        var result = Repository.ById<ArbitrationResultDto>(id);
        result.CanSign = StatusIdConst.CanArbitrationResultApplyStatus(result.StatusId, StatusIdConst.SIGNED);
        result.CanEdit = StatusIdConst.CanArbitrationResultApplyStatus(result.StatusId, StatusIdConst.MODIFIED);
        result.CanDelete = StatusIdConst.CanArbitrationResultApplyStatus(result.StatusId, StatusIdConst.DELETED);


        foreach (var file in result.Files)
        {
            if (file.FileExtension == ".docx" || file.FileExtension == ".doc")
            {
                file.FileExtension = ".pdf";
                file.FileName = file.FileName.Replace(".docx", ".pdf");
                file.FileName = file.FileName.Replace(".doc", ".pdf");
            }
        }
        return result;
    }
    public override PagedResult<ArbitrationResultListDto> GetList(
        ArbitrationResultSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<ArbitrationResultListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }
    public async Task<HaveId<long>> Sign(SignStatusArbitrationResultDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ??
            await UnitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            var entity = Repository.AllAsQueryable
                   .Include(x => x.Signs)
                   .ThenInclude(x => x.ArbitrationJudge)
                   .ThenInclude(x => x.Person)
                   .FirstOrDefault(x => x.Id == dto.Id);

            //var judge = UnitOfWork.ArbitrationJudgeRepository.AllAsQueryable.Include(x => x.Person).ToList();

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
                        .FirstOrDefault(x => x.Id == entity.Id);
            var signer = entity.Signs.FirstOrDefault(x =>
                x.ArbitrationJudge.Person.Pinfl == eImzoTimstampDto.Pinfl);

            if (signer == null)
            {
                AddError("Нет доступ");
            }
            var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

            CombineStatuses(_eImzoService);
            if (HasErrors)
            {
                transaction.Rollback();
                return null;
            };


            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = null,
                Pinfl = _authService.Contractor == null ? _authService.User.Pinfl : _authService.Contractor.Pinfl
            });

            //var signer = new ArbitrationResultSign()
            //{
            signer.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
            signer.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
            signer.SignedAt = DateTime.Now;
            signer.SignedUserInfo = _authService.User != null
                ? _authService.User.ToTextForDocumentLog()
                : _authService.Contractor.FullName + " - " + _authService.Contractor.Inn;
            signer.StatusId = dto.StatusId;
            //};
            //entity.Signs.Add(signer);

            if (!entity.Signs.Any(x => x.SignedAt == null))
            {
                dto.StatusId = StatusIdConst.SIGNED;
            }

            if (dto.StatusId != entity.StatusId)
            {
                var ent = Repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, dto.StatusId))
                        Repository.AddError("Нет доступа");
                });
                CombineStatuses(Repository);
                if (HasErrors)
                {
                    await transaction.RollbackAsync();
                    return null;
                }
            }
            _arbitrationCourtApplicationService.CourtDecisionStep(new()
            {
                Id = entity.ArbitrationCourtApplicationId,
                ApplyFilter = false
            });

            CombineStatuses(_arbitrationCourtApplicationService);
            if (HasErrors)
            {
                await transaction.RollbackAsync();
                return null;
            }

            await UnitOfWork.Context.SaveChangesAsync();

            if (IsValid && canCommit)
                await transaction.CommitAsync();
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
                var statusDto = new UpdateStatusArbitrationResultDlDto()
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
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteArbitrationResult");

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
    private HaveId<long> UpdateStatus(UpdateStatusArbitrationResultDlDto dto, Action<ArbitrationResult> validation)
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
            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message: "ArbitrationResult");
            if (IsValid && canCommit)
                transaction.Commit();
            return res;
        }
        finally
        {
            transaction?.Dispose();
        }
    }
    private Action<ArbitrationResult> Validation(UpdateStatusArbitrationResultDlDto dto)
    {
        return ent =>
        {
            if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, dto.StatusId))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<ArbitrationResultDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
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

    public void Cancel(CancelStatusArbitrationResultDto dto)
    {
        var canCommit = UnitOfWork.CurrentTransaction == null;
        var transaction = UnitOfWork.CurrentTransaction ?? UnitOfWork.BeginTransaction();

        try
        {
            UnitOfWork.Context.Set<ArbitrationResult>().Lock(dto.Id);

            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationDiscussionApplyStatus(ent.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа");
            });

            CombineStatuses(Repository);
            if (HasErrors)
                return;

            UnitOfWork.Save();

            var res = CreateDocumentChangeLog(id: dto.Id, message: dto.Message);

            if (IsValid && canCommit)
                transaction.Commit();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    #region Files Downloands
    private async Task<byte[]> DownloadPdf(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        var storageFile = Download(fileId, entity, storageDocument);
        var stream = storageFile.GetStream();
        MemoryStream wordFile = new();
        await wordFile.WriteAsync(stream.ReadAsBytes());
        var doc = Repository.ReadAsNoTracked<ArbitrationResultDto>(x => x.Files.Any(f => f.Id == fileId))
            .FirstOrDefault();
        if (doc == null)
        {
            AddError("Hujjat topilmadi.");
            return null;
        }
        var plh = WordFactory.MakePlaceholders(doc);
        #region QrCodes

        //var signSsp = UnitOfWork.Context
        //        .Set<ArbitrationResultSign>()
        //        .FirstOrDefault(s => s.OwnerId == doc.Id);
        //      var signContractor = UnitOfWork.Context
        //          .Set<AdditionalAgreementSign>()
        //      .FirstOrDefault(s =>
        //      s.OwnerId == doc.Id
        //          && s.StatusId == StatusIdConst.SIGNED);


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
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_ARBITRATION_RESULT, files)
            .Select(a => new ArbitrationResultFileDto
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
            $"{nameof(TableIdConst.CLAIM__DOC_ARBITRATION_APPLICATION)}_SIGN_DATA",
            docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
    private HaveId<long> CreateDocumentChangeLog(
        long id,
        string message = null,
        string userIp = null,
        string userAgent = null)
    {
        var entityDto = Repository.ById<ArbitrationCourtApplicationDto>(
            id, applyFilter: false);
        _documentChangeLogService.CreateApplication(
            dto: entityDto,
            organizationId: null,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = UnitOfWork.Context.Set<ArbitrationResultFile>().FirstOrDefault(a => a.Id == fileId);
        StorageFile file;
        if (entity == null)
            AddError("file not found");
        if (entity.FileExtension.Contains("docx"))
        {
            file = new(
                fileId: fileId,
                fileName: entity.FileName,
                new MemoryStream(DownloadPdf(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_RESULT).Result));
        }
        else
            file = Download(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_RESULT);

        return file;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = UnitOfWork.Context
            .Set<ArbitrationCourtApplicationFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_RESULT);
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
