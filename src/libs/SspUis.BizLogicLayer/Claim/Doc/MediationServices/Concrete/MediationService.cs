using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.MediationServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Claim;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using WEBASE.i18n;
using WEBASE.Models;
using WEBASE.OfficeTools;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Claim;

public class MediationService : StatusGenericHandler, IMediationService
{
    private readonly IMediationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IStorageService _storageService;
    private readonly INumberService _numberService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly ICultureHelper _cultureHelper;
    private readonly IConvertService _pdfConverter;
    private readonly SystemConf _systemConf;
    private readonly IMediationPlanService _mediationPlanService;
    private readonly IClaimApplicationRepository _claimApplicationRepository;
    public MediationService(
        IAuthService authService,
        IUnitOfWork unitOfWork,
        IStorageService storageServic,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        ICultureHelper cultureHelper,
        IConvertService pdfConverter,
        SystemConf systemConf,
        IMediationPlanService mediationPlanService,
        IClaimApplicationRepository claimApplicationRepository)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _repository = unitOfWork.MediationRepository;
        _storageService = storageServic;
        _numberService = numberService;
        this._documentChangeLogService = documentChangeLogService;
        this._cultureHelper = cultureHelper;
        this._pdfConverter = pdfConverter;
        this._systemConf = systemConf;
        this._mediationPlanService = mediationPlanService;
        _claimApplicationRepository = claimApplicationRepository;
    }

    public PagedResult<MediationListDto> GetList(MediationSortFilterOptions options)
    {
        var result = _repository.ReadAsNoTracked<MediationListDto>()
                    .SortFilter(options)
                    .ToTableData(options);

        if (options.IsEmployee)
            result.Rows = result.Rows.Where(x => x.EmployeeManageId == _authService.User.EmployeeManageId);

        return result;
    }
    public MediationDto Get()
    {
        return new MediationDto()
        {
            DocNumber = _numberService.GetNext(
            nameof(TableIdConst.CLAIM__DOC_MEDIATION),
            organizationId: _authService.IsAuthenticated ? _authService.Organization.Id : OrganizationIdConst.SSP)
            .Item2,
            TableId = TableIdConst.CLAIM__DOC_MEDIATION
        };
    }
    public MediationDto GetByPlanId(int planId)
    {
        var plan = _unitOfWork.Context.Set<MediationPlan>()
            .Include(a => a.Application).ThenInclude(a => a.ClaimApplication).ThenInclude(a => a.Tables).ThenInclude(a => a.ClaimResponsibleType)
            .Include(a => a.Application.ClaimApplication.ClaimTheme)
            .Include(p => p.Contractor)
            .Include(p => p.MeetingType)
            .AsSplitQuery()
            .FirstOrDefault(p => p.Id == planId);

        if (plan == null)
        {
            AddError("Plan topilmadi");
            return null;
        }

        var res = new MediationDto()
        {
            MediationPlanId = plan.Id,
            ContractorId = plan.ContractorId,
            ContractorInn = plan.Contractor.Inn,
            Contractor = plan.Contractor.FullName,
            ChamberPerson = plan.ChamberPerson,
            PlanDocNumber = plan.DocNumber,
            PlanDocDate = plan.MeditionAt,
            MeetingType = plan.MeetingType.FullName,
            MeetingTypeId = plan.MeetingType.Id,
            ClaimTheme = plan.Application.ClaimApplication.ClaimTheme.FullName,
            ClaimThemeId = plan.Application.ClaimApplication.ClaimTheme.Id,
            StepId = plan.Application.CurrentStepId,
            DocNumber = plan.DocNumber,
            Table = plan.Application.ClaimApplication.Tables.Select(a => new MediationClaimApplicationTableDto
            {
                OrderNumber = a.OrderNumber,
                Address = a.Address,
                FullName = a.FullName,
                InnOrPinfl = a.InnOrPinfl,
                PhoneNumber = a.PhoneNumber,
                IsRegistred = a.IsRegistred,
                ClaimResponsibleTypeId = a.ClaimResponsibleTypeId,
                ClaimResponsibleType = a.ClaimResponsibleType.FullName,
            }).ToList(),
        };
        return res;
    }
    public MediationDto Get(long id)
    {
        var dto = _repository.ById<MediationDto>(id);

        CombineStatuses(_repository);

        return dto;
    }
    public HaveId<long> Create(CreateMediationDlDto dto)
    {
        if (dto.ClaimNeedCourtId == 3 && dto.CourtAt == null)
        {
            AddError("Qayta ko'rib chiqilishi kerak bo'sa, sud sanasi belgilanishi shart");
            return null;
        }

        if (_repository.Context.Set<Mediation>()
            .Any(x => x.MediationPlanId == dto.MediationPlanId))
        {
            AddError(" Bu " + nameof(dto.MediationPlanId) + " mavjud");
            return null;
        }
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.Create(dto, ent => Validation(dto, ent));
                CombineStatuses(_repository);
                if (HasErrors) return null;

                if (IsValid) _unitOfWork.Save();

                SaveFiles(entity, dto.Files);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateMediation");

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch (DbException ex)
            {
                AddError($"{ex.Message} || {ex.InnerException}");
                transaction.Rollback();
                throw;
            }
        }
    }
    public void Update(UpdateMediationDlDto dto)
    {
        var entity = _repository.Update(dto);
        var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateMediation");

        CombineStatuses(_repository);
        if (IsValid)
            _unitOfWork.Save();
    }
    public HaveId<long> Accept(CreateMediationPlanIFMeditionReviewDlDto dto)
    {
        var updateStatusDlDto = new UpdateStatusMediationDlDto()
        {
            Id = dto.Id,
            StatusId = StatusIdConst.ACCEPTED
        };

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var ent = _repository.UpdateStatus(updateStatusDlDto, ent =>
                {
                    if (ent is null) AddError("Не найдено");

                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, updateStatusDlDto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                CombineStatuses(_repository);
                if (HasErrors) return null;

                _unitOfWork.Save();

                if (ent.MediationResultId == MediationResultIdConst.GIVEN_TIME && ent.ClaimNeedCourtId == ClaimNeedCourtIdConst.REVIEWING)
                {
                    if (dto.DocOn == null || dto.MeetingTypeId == null || dto.AddressOrUrl == null || dto.ChamberPerson == null)
                    {
                        AddError("Бу документ тури муддат берилган. Бунга янги Медиация pежаси яратишга мажбурсиз !");
                        return null;
                    }
                    if (dto.DocOn <= ent.DocOn)
                    {
                        AddError($"Медиация баённомасига муддат бераётганда янги медиация режаси санаси: {dto.DocOn} баённома санасидан: {ent.DocOn} катта бўлиши керак !");
                        return null;
                    }

                    var newPlanId = _mediationPlanService.Create(new CreateMediationPlanDlDto
                    {
                        DocNumber = ent.DocNumber,
                        DocOn = dto.DocOn.Value,
                        ApplicationId = ent.MediationPlan.ApplicationId,
                        MeetingTypeId = dto.MeetingTypeId.Value,
                        AddressOrUrl = dto.AddressOrUrl,
                        ContractorId = ent.ContractorId,
                        MeditionAt = dto.MeditionAt ?? DateTime.Now,
                        ChamberPerson = dto.ChamberPerson
                    });
                    CombineStatuses(_mediationPlanService);
                    if (HasErrors) return null;

                    ent.MediationPlan.NextMediationPlanId = newPlanId.Id;
                }

                _claimApplicationRepository.UpdateStep(
                    new UpdateStepDlDto
                    {
                        Id = ent.MediationPlan.ApplicationId,
                        StepId = ent.MediationResultId == MediationResultIdConst.NO_AGREEMANT_REACHED 
                            ? StepIdConst.MEDIATION_CREATE
                            : StepIdConst.MEDIATION_PLAN_CREATE,
                    });

                CombineStatuses(_claimApplicationRepository);
                if (HasErrors) { transaction.Rollback(); return null; }

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(ent.Id, ent.StatusId);

                if (IsValid) transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message} // {ex.InnerException}");
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
            }
        }
        return null;
    }
    public HaveId<long> Cancel(long id, string? message)
    {
        if (_unitOfWork.Context.Set<ApplicationForCourt>().Any(a => a.MediationId == id))
        {
            AddError("Already ApplicationForCourt created");
            return null;
        }

        var dto = new UpdateStatusMediationDlDto()
        {
            Id = id,
            StatusId = StatusIdConst.CANCELED
        };

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.UpdateStatus(dto, ent =>
                {
                    if (ent == null) AddError("Not found");

                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                CombineStatuses(_repository);
                if (HasErrors) return null;

                _claimApplicationRepository.UpdateStep(
                    new UpdateStepDlDto
                    { 
                        Id = entity.MediationPlan.ApplicationId,
                        StepId = StepIdConst.MEDIATION_CANCEL
                    });

                CombineStatuses(_claimApplicationRepository);
                if (HasErrors) return null;

                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, message);

                if (IsValid) transaction.Commit();
                return res;
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
        return null;

    }
    public void Delete(long id)
    {
        try
        {
            _repository.Delete(id);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        catch (DbUpdateException)
        {
            AddError("Запись не может быть удален");
        }
    }
    public byte[] DownloadPdf(Guid id2, string? lang)
    {
        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(
                    lang,
                     StaticFileConst.WordTemplate.MEDIATION)
             );
        var lan = _unitOfWork.Context.Set<Language>()
            .FirstOrDefault(l => l.Code == lang)?.Id ?? 1;

        var mediation = _unitOfWork.Context.Set<Mediation>()
            .Include(m => m.Contractor)
            .Include(m => m.MediationPlan)
            .Include(m => m.ClaimNeedCourt)
            .Include(m => m.MediationResult)
            .FirstOrDefault(m => m.Id2 == id2);
        if (mediation == null)
        {
            AddError("Mediatsiyta bayoni topilmadi.");
            return null;
        }
        var tables = _unitOfWork.Context.Set<ClaimApplicationTable>()
            .Where(a => a.Owner.ApplicationId == mediation.MediationPlan.ApplicationId);

        var creator = _unitOfWork.Context.Set<User>()
            .Include(u => u.Person)
            .FirstOrDefault(u => u.Id == mediation.CreatedUserId);
        var plh = new Placeholders();
        var link = _systemConf.QrImagePrintMy + "/Mediation/DownloadPdf?id2=" + mediation.Id2.ToString();
        var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        plh.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });
        plh.TextPlaceholders.Add(nameof(mediation.DocOn), mediation.MediationPlan.MeditionAt.ToString("dd.MM.yyyy") ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.DocNumber), mediation.DocNumber ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.MediationPlan.MeditionAt), mediation.MediationPlan.MeditionAt.ToString("hh:mm") ?? "");
        plh.TextPlaceholders.Add(nameof(creator.UserName), creator?.Person.FullName ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.Contractor) + nameof(mediation.Contractor.Director), mediation.Contractor.Director ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.Contractor) + nameof(mediation.Contractor.FullName), mediation.Contractor.FullName ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.ContractorDetails), mediation.ContractorDetails ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.ResponsibleDetails), mediation.ResponsibleDetails ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.ResponsiblePersonName), mediation.ClaimantPersonName ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.ClaimantPersonName), mediation.ResponsiblePersonName ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.MediationPlan.Application.ClaimApplication.Tables), string.Join(", ", tables.Select(t => t.FullName)) ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.MediationResult), mediation?.MediationResult?.FullName ?? "");
        plh.TextPlaceholders.Add(nameof(mediation.ClaimNeedCourt), mediation.ClaimNeedCourt?.FullName ?? "");
        string courtData = string.Empty;

        if (mediation.ClaimNeedCourtId == ClaimNeedCourtIdConst.REVIEWING || mediation.MediationResultId == MediationResultIdConst.GIVEN_TIME)
        {
            courtData =
                $"Низони судгача ҳал қилиш бўйича ўтказилган такрорий учрашув " +
                $"{mediation.CourtAt?.Year.ToString() ?? ""}" +
                $"йил \"{mediation.CourtAt?.Day.ToString() ?? ""}\" " +
                $"{mediation.CourtAt?.Month.ToString() ?? ""} ой санаси соат " +
                $"{mediation.CourtAt?.ToString("HH:mm") ?? ""} га қолдирилади.";
        }
        plh.TextPlaceholders.Add(nameof(mediation.CourtAt.Value.Hour), courtData);

        wordFile = (new DocXHandler(wordFile, plh)).ReplaceAll();
        var res = _pdfConverter.DocxToPdfAsync(wordFile, new()).Result;
        CombineStatuses(_pdfConverter);
        return res;
    }
    public IEnumerable<MediationFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.MEDIATION_FILES, files).Select(a => new MediationFileDto
        {
            Id = a.FileId,
            FileName = a.FileName
        });
        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<MediationFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.MEDIATION_FILES);
    }
    private void Delete(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        if (entity != null)
        {
            _storageService.DeleteTemp(storageDocument, fileId);
            CombineStatuses(_storageService);
        }
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<MediationFile>().FirstOrDefault(a => a.Id == fileId);
        return Download(fileId, entity, DocumentStorageConst.MEDIATION_FILES);
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
    private void SaveFiles(Mediation entity, List<MediationFileDlDto> files)
    {
        if (files != null)
        {
            _storageService.MoveToPersistent(DocumentStorageConst.DOC_MEDIATION_FILE, $"{entity.Id}", files.Select(a => a.Id).ToArray());
            CombineStatuses(_storageService);
        }
    }
    private HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = _repository.ById<MediationDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.CLAIM__DOC_MEDIATION,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(MediationDlDto<TDto> dto, Mediation entity)
            where TDto : MediationDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);
        }
    }
}
