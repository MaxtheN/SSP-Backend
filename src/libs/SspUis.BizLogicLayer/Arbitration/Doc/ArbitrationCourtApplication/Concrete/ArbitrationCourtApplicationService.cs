using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.QRCode;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;

public class ArbitrationCourtApplicationService
    : BaseApplicationService
        <ArbitrationCourtApplication,
        ArbitrationCourtApplicationListDto,
        ArbitrationCourtApplicationDto,
        CreateArbitrationCourtApplicationDlDto,
        UpdateArbitrationCourtApplicationDlDto,
        IArbitrationCourtApplicationRepository,
        ArbitrationCourtApplicationSortFilterOptions>
    , IArbitrationCourtApplicationService
{

    #region ctor
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IEImzoService _eImzoService;
    private readonly INumberService _numberService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IStorageService _storageService;
    private readonly SystemConf _systemConf;
    private readonly IContractorService _contractorService;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;
    private readonly IConvertService _pdfConverter;

    public ArbitrationCourtApplicationService(
        IUnitOfWork unitOfWork,
        IAuthService authService,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IEImzoService eImzoService,
        SystemConf systemConf,
        IConvertService pdfConverter,
        IStorageService storageService,
        WbImzoConfig wbImzoConfig,
        List<LinkConfig> linkConfigs,
        IWbImzoService wbImzoService,
        IContractorService contractorService)
        : base(unitOfWork, documentChangeLogService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
        _numberService = numberService;
        _documentChangeLogService = documentChangeLogService;
        _storageService = storageService;
        _eImzoService = eImzoService;
        _systemConf = systemConf;
        _pdfConverter = pdfConverter;
        this._contractorService = contractorService;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "ArbitrationCourtApplication");
    }
    #endregion

    #region CRUD
    protected override IQueryable<ArbitrationCourtApplicationListDto> SortFilter(
        IQueryable<ArbitrationCourtApplicationListDto> query,
        ArbitrationCourtApplicationSortFilterOptions options)
    {
        return base.SortFilter(query, options)
            .SortFilter(options)
            .Where(a => a.Application.ApplicationTypeId == ApplicationTypeIdConst.ARBITRATION);
    }
    public override PagedResult<ArbitrationCourtApplicationListDto> GetList(
        ArbitrationCourtApplicationSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<ArbitrationCourtApplicationListDto>()
            .SortFilter(options)
            .AsPagedResult(options);
    }

	public int GetCount()
	{
        ArbitrationCourtApplicationSortFilterOptions options = 
                new ArbitrationCourtApplicationSortFilterOptions();
		var data = Repository.ReadAsNoTracked<ArbitrationCourtApplicationListDto>()
			.SortFilter(options);

        return data.Count();
	}
	public async Task<ArbitrationCourtApplicationDto> Get()
    {
        var firstStep = Repository.CrudServices.ProjectFromEntityToDto<ApplicationTypeStep, ApplicationTypeStepDto>(query =>
            query.Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.ARBITRATION)).FirstOrDefault(x => x.Id == StepIdConst.CLAIMED);
        if (_authService.Contractor == null)
            return new()
            {
                Application = new()
                {
                    ApplicationTypeId = ApplicationTypeIdConst.ARBITRATION,
                    DocOn = DateTime.Now.AsDateOnly(),
                    ContractorPositionName = "",
                    CurrentStep = firstStep,
                    DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, OrganizationIdConst.SSP).Item2,
                },
            };
        else
        {
            return BuildNewModelForFront(_authService.Contractor.Id);
        }
    }

    private ArbitrationCourtApplicationDto BuildNewModelForFront(long contractorId)
    {
        if (contractorId == 0)
        {
          
        }

        var contractor = UnitOfWork.Context
                            .Set<Contractor>()
                            .Include(x => x.Region).ThenInclude(x => x.Translates)
                            .Include(x => x.District).ThenInclude(x => x.Translates)
                            .FirstOrDefault(x => x.Id == contractorId);
        

        var firstStep = Repository.CrudServices.ProjectFromEntityToDto<ApplicationTypeStep, ApplicationTypeStepDto>(query =>
          query.Where(x => x.ApplicationTypeId == ApplicationTypeIdConst.ARBITRATION)).FirstOrDefault(x => x.Id == StepIdConst.CLAIMED);

        return new()
        {
            Application = new()
            {
                DocOn = DateTime.Now.AsDateOnly(),
                ContractorPositionName = contractor.FullName,
                ApplicationTypeId = ApplicationTypeIdConst.ARBITRATION,
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPLICATION, OrganizationIdConst.SSP).Item2,
                ContractorId = contractor.Id,
                ContractorAddress = contractor.Address,
                ContractorDirector = contractor.Director,
                ContractorInn = contractor.Inn,
                Contractor = contractor.FullName,
                RegionId = contractor.RegionId,
                DistrictId = contractor.DistrictId,
                CurrentStep = firstStep,
                Region = contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? contractor.Region.FullName,
                District = contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? contractor.District.FullName,
            },
            ECourtNumber = _numberService.GetNext(
                NumberTemplateDocumentConst.DOC_APPLICATION,
                OrganizationIdConst.SSP,
                contractor.RegionId,
                contractor.DistrictId).Item2,
            ContractorPhonber = contractor.PhoneNumber,
            ContractorId = contractor.Id,
            ContractorAddress = contractor.Address,
            ContractorInnPnfl = contractor.Inn ?? contractor.Pinfl
        };
    }

    public ArbitrationCourtApplicationDto GetByContractorId(long contractorId)
    {
		return BuildNewModelForFront(contractorId);
    }

    public override ArbitrationCourtApplicationDto Get(long id)
    {
        var dto = Repository.ById<ArbitrationCourtApplicationDto>(id);
        if (dto != null && dto.IsCreatedByErp)
        {
            var createdUser = UnitOfWork.Context.Users.Include(x => x.Person).FirstOrDefault(x => x.Id == dto.CreatedUserId);
            if (createdUser != null)
                dto.CreatedUser = createdUser.Person.FullName ?? createdUser.UserName;
            else
            {
                var businessmanUser = UnitOfWork.Context.BusinessmanUsers.FirstOrDefault(x => x.Id == dto.CreatedUserId);
                dto.CreatedUser = businessmanUser?.FullName;
            }
        }

        if (_authService.Contractor != null)
        {
            dto.CanSetJudge = dto.Application.CurrentStep.Id == StepIdConst.NOTIFIED;
        }
        else
        {
            dto.CanCreateDiscussionDoc = dto.Application.CurrentStep.Id == StepIdConst.NOTIFIED
                && dto.ArbitrationDiscussionId == null
                && (_authService.User.OrganizationId == OrganizationIdConst.SSP 
                || _authService.User.OrganizationId == OrganizationIdConst.COURT_OF_ARBITRATION);

            dto.CanCreateDelayDoc = dto.Application.CurrentStep.Id == StepIdConst.NEED_DISCUSSION
                && dto.ArbitrationDelayId == null
                ;

            dto.CanCreateResultDoc = (dto.Application.CurrentStep.Id == StepIdConst.DELAYED
                || dto.Application.CurrentStep.Id == StepIdConst.NEED_DISCUSSION);

            dto.CanChangeStep = dto.Application.CurrentStep.Id == StepIdConst.CLAIMED
                /*|| dto.Application.CurrentStep.Id == StepIdConst.NEED_DISCUSSION*/;

            dto.CanSetJudge = dto.Application.CurrentStep.Id == StepIdConst.NOTIFIED;
            dto.CanDelete = StatusIdConst.CanArbitrationCourtApplicationApplyStatus(dto.Application.StatusId, StatusIdConst.DELETED);
            dto.HasArbitrationResult = dto.ArbitrationResultId.HasValue;
            dto.HasArbitrationDiscussion = dto.ArbitrationDiscussionId.HasValue;
            dto.HasArbitrationDelay = dto.ArbitrationDelayId.HasValue;
        }
        return dto;
    }
    public override HaveId<long> Create(CreateArbitrationCourtApplicationDlDto dto)
    {
        if (dto.IsForeignResponsible && dto.ResponsibleContractorId != null)
        {
            AddError("Javobgarni ma'lumoti to'liq emas ");
            return null;
        }
        if (dto.ContractorId == dto.ResponsibleContractorId)
        {
            AddError("Davogar va javobgar bir hil");
            return null;
        }
        else if (dto.IsForeignResponsible == true && dto.IsForeignContractor == true)
        {
            AddError("Davogar va javobgar chet'elik ");
            return null;
        }

        if ((dto.ContractorId == null && dto.ContractorInn == null) && (!dto.IsForeignContractor))
        {
            AddError("Contractor is empty!!!");
            return null;
        }
        if ((dto.ResponsibleContractorId == null &&
            dto.ResponsibleInn == null) &&
            (!dto.IsForeignResponsible))
        {
            AddError("Responsible is empty!!!");
            return null;
        }
        var bhm = UnitOfWork.FixedMinimumValueRepository.AllAsQueryable
            .FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);

        if (dto.ArbitrationAmount < bhm.FixedValue * 3)
        {
            AddError($"Hakamlik yig'imi kamida bazaviy hisoblash miqdori({bhm.FixedValue})ning uch baravari miqdorida bo'lishi kerak");
        }
        //if (_authService.Contractor == null && dto.SignedData == null)
        //{
        //    AddError("E-imzo bilan tasdiqlash kerak.");
        //    return null;
        //}

        for (int i = 0; i < dto.Files.Count; i++)
        {
            // ArbitrationCourtApplicationFileDlDto item = dto.Files[i];
            dto.Files[i].StepId = StepIdConst.CLAIMED;
        }
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                if (dto.ContractorId == null)
                {
                    if (dto.IsForeignContractor)
                        dto.ContractorId = null;
                    else
                    {
                        long id = CreateContractorFromSoliq(dto.ContractorInn, dto.IsForeignContractor);
                        dto.ContractorId = (id == 0) ? null : id;
                    }
                }
                if (dto.ResponsibleContractorId == null)
                {
                    if (dto.IsForeignResponsible)
                        dto.ResponsibleContractorId = null;
                    else
                    {
                        long id = CreateContractorFromSoliq(dto.ResponsibleInn, dto.IsForeignResponsible);
                        dto.ResponsibleContractorId = (id == 0) ? null : id;
                    }
                }

                var entity = Repository.Create(dto);
                CombineStatuses(Repository);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                entity.Application.CurrentStepId = StepIdConst.CLAIMED;

                UnitOfWork.Save();

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }
                _storageService.MoveToPersistent(DocumentStorageConst.DOC_ARBITRATION_APPLICATION, entity.Id.ToString(), dto.Files.Select(x => x.Id).ToArray());
                var res = CreateDocumentChangeLog(entity.Id);

                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

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

    private long CreateContractorFromSoliq(string inn, bool isForeigner)
    {
        long contractorId = 0;
        if (inn == null)
        {
            AddError("inn is null");
            return 0;
        }
        DataLayer.EfClasses.Contractor contractor = _unitOfWork.ContractorRepository.ByInn(inn);

        if (contractor != null)
            contractorId = contractor.Id;
        else
        {
            var contractorDto = _contractorService.GetFromSoliq(inn);
            var mc = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ContractorDto, CreateContractorDlDto>();
            });
            var createContractorDlDto = mc.CreateMapper().Map<CreateContractorDlDto>(contractorDto);

            var contractorEntity = _contractorService.Create(createContractorDlDto);

            CombineStatuses(_contractorService);
            if (HasErrors)
                return 0;
            _unitOfWork.Save();
            contractorId = contractorEntity.Id;
        }
        return contractorId;
    }
    public HaveId<long> Update(UpdateArbitrationCourtApplicationDlDto dto)
    {
        #region

        #endregion

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent =>
                {
                    if (!StatusIdConst.CanEditStatuses.Contains(ent.Application.StatusId))
                        AddError("Tahrirlash mumkin emas / Невозможно редактировать");
                });

                CombineStatuses(Repository);

                if (IsValid)
                    UnitOfWork.Save();

                _storageService.ResolveMarkedFiles(DocumentStorageConst.DOC_ARBITRATION_APPLICATION, dto.Id.ToString());
                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid)
                    transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AddError(ex.Message + " Inner: " + ex.InnerException);
                return null;
            }
        }
    }
    public override void Delete(long id)
    {

        var statusDto = new UpdateStatusArbitrationCourtApplication()
        {
            Id = id,
            StatusId = StatusIdConst.DELETED
        };

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanArbitrationCourtApplicationApplyStatus(ent.Application.StatusId, statusDto.StatusId))
                        AddError("Нет доступа");
                });
                CombineStatuses(Repository);

                if (IsValid)
                    UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id);

                if (IsValid)
                    transaction.Commit();

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                AddError(ex.Message + " Inner: " + ex.InnerException);
            }
        }
    }
    #endregion


    #region Accept Cancel 
    public HaveId<long> NextStep(long id)
    {
        var entity = Repository.AllAsQueryable
            .Include(x => x.Application)
            .Include(x => x.Signer)
            .FirstOrDefault(x => x.Id == id);
        if (entity == null)
        { AddError("Ariza topilmadi. :("); return null; }
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction
            ?? _unitOfWork.BeginTransaction();
        var discussion = UnitOfWork.ArbitrationDiscussionRepository.AllAsQueryable
            .Include(x => x.Signs).FirstOrDefault(x => x.ArbitrationCourtApplicationId == entity.Id);

        try
        {
            if (entity.Application.CurrentStepId == StepIdConst.CLAIMED)
            {
                NotifiedStep(new()
                {
                    Id = entity.Id,
                });
            }
            else if (entity.Application.CurrentStepId == StepIdConst.NEED_DISCUSSION)
            {
                DelayedStep(new()
                {
                    Id = entity.Id,
                });
            }
            else
            {
                AddError("Вы не можете изменить шаг вручную!!!");
                return null;
            }
            UnitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            if (canCommit)
                transaction.Rollback();
            AddError(ex.Message + " Inner: " + ex.InnerException);
            return null;
        }

    }
    public void Reject(RejectStatusArbitrationCourtApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<Application>().Lock(dto.Id);

            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationCourtApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа");
                else
                {
                    ent.Application.StatusId = dto.StatusId;
                    ent.Application.Message = dto.Message;
                }
            });

            CombineStatuses(Repository);
            if (HasErrors)
                return;
            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(id: dto.Id);

            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            if (canCommit)
                transaction.Rollback();
            AddError(ex.Message + " Inner: " + ex.InnerException);
        }
    }
    public void Accept(AcceptStatusArbitrationCourtApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<Application>().Lock(dto.Id);

            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationCourtApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа");
                else
                {
                    ent.Application.StatusId = dto.StatusId;
                    ent.Application.Message = dto.Message;
                }
            });
            CombineStatuses(Repository);
            if (HasErrors)
                return;
            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(dto.Id);
            if (IsValid && canCommit)
                transaction.Commit();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public void Cancel(CancelStatusArbitrationCourtApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            _unitOfWork.Context.Set<Application>().Lock(dto.Id);

            Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationCourtApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа");
                else
                {
                    ent.Application.StatusId = dto.StatusId;
                    ent.Application.Message = dto.Message;
                }
            });

            CombineStatuses(Repository);
            if (HasErrors)
                return;

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(id: dto.Id);

            if (IsValid && canCommit)
                transaction.Commit();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public async Task Send(SendStatusArbitrationCourtApplicationDto dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {
            var res = Repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanArbitrationCourtApplicationApplyStatus(ent.Application.StatusId, dto.StatusId))
                    AddError("Имкони йўқ / Нет доступа :( ");
            });

            CombineStatuses(Repository);
            if (IsValid && canCommit)
                transaction.Commit();
            UnitOfWork.Save();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            transaction.Rollback();
        }
    }
    public async Task Sign(SignStatusArbitrationCourtApplicationDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.AllAsQueryable
                    .Include(x => x.Signer)
                    .ThenInclude(x => x.ArbitrationJudge)
                    .FirstOrDefault(x => x.Id == dto.Id);

                if (entity == null)
                {
                    AddError("Not found");
                    transaction.Rollback();
                    return;
                }


                ArbitrationCourtApplicationSigner signer = entity.Signer
                    .FirstOrDefault(x => x.ArbitrationJudge.PersonId == _authService.User.PersonId
                        && x.SignedAt == null);

                if (signer == null)
                {
                    if (entity.Signer.Any(x => x.EmployeeManageId != null))
                    {
                        AddError("Bu hujjat imzolangan");
                    }
                    else
                    {
                        if (_authService.User.EmployeeManageId == null)
                        {
                            AddError("Hodimlar boshqaruvidan sizni topa olmadik.  ;(");
                        }
                        var employeeManage = _unitOfWork.EmployeeManageRepository.AllAsQueryable
                        .FirstOrDefault(x => x.Id == _authService.User.EmployeeManageId.Value
                        && x.EndOn == null && x.IsDeleted == false);

                        if (employeeManage == null)
                        {
                            AddError("Hodimlar boshqaruvidan sizni topa olmadik.  ;(");
                        }
                        if (entity.Signer.Any(x => x.SignedAt == null))
                        {
                            AddError("Bu hujjatda imzolar to'liq emas");
                        }
                        signer = new()
                        {
                            SignOrder = entity.Signer.Max(x => x.SignOrder) + 1,
                            DepartmentId = employeeManage.DepartmentId,
                            PositionId = employeeManage.PositionId,
                            EmployeeManageId = employeeManage.Id
                        };

                        entity.Signer.Add(signer);
                    }
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }
                }

                var eImzoTimstampDto = new EImzoTimeStampDto
                {
                    SignData = dto.SignedData,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                };
                var app = UnitOfWork.ArbitrationCourtApplicationRepository.AllAsQueryable
                    .Include(x => x.Signer)
                    .ThenInclude(x => x.ArbitrationJudge)
                        .FirstOrDefault(x => x.Id == entity.Id);

                if (!app.Signer
                    .Any(x => x.ArbitrationJudge.Person.Pinfl == eImzoTimstampDto.Pinfl))
                {
                    AddError("Нет доступ");
                }
                var timeStamp = await _eImzoService.TimeStamp(eImzoTimstampDto);

                CombineStatuses(_eImzoService);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                };

                var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
                {
                    SignData = timeStamp.Pkcs7b64,
                    Inn = null,
                    Pinfl = _authService.User.Pinfl
                });

                signer.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
                signer.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
                signer.SignedAt = DateTime.Now;
                signer.SignedUserInfo = _authService.User.ToTextForDocumentLog();

                _unitOfWork.Save();
                if (IsValid)
                    transaction.Commit();

            }
            catch (DbUpdateException ex)
            {
                AddError(ex.Message + ", " + ex.InnerException);
                transaction.Rollback();
            }
        }
    }
    #endregion


    #region STEP CONTROLL
    private void NotifiedStep(NotifiedStepArbitrationCourtApplication dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {

            UpdateStep(dto, ent => { });
            if (IsValid)
                _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public void DiscussionStep(DiscussionStepArbitrationCourtApplication dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {

            UpdateStep(dto, ent => { });
            if (IsValid)
                _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public void DelayedStep(DelayedStepArbitrationCourtApplication dto)
    {
        var app = _unitOfWork.Context.Set<ArbitrationCourtApplication>()
            .Include(x => x.Application)
            .FirstOrDefault(x => x.Id == dto.Id);
        //dto.Id = app.ApplicationId;
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {

            UpdateStep(dto, ent => { }, dto.ApplyFilter);
            if (IsValid)
                _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    public void CourtDecisionStep(CourtDecisionStepArbitrationCourtApplication dto)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        try
        {

            UpdateStep(dto, ent => { }, dto.ApplyFilter);
            if (IsValid)
                _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
        }
        catch (DbUpdateException e)
        {
            AddError(e.Message + " - " + e.InnerException);
            if (canCommit)
                transaction.Rollback();
        }
        finally
        {
            if (canCommit)
                transaction.Dispose();
        }
    }
    //public void StepFife(ArbitrationCourtApplication entity)
    //{
    //    var canCommit = _unitOfWork.CurrentTransaction == null;
    //    var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

    //    try
    //    {

    //        UpdateStep(dto, ent => { });
    //        if (IsValid)
    //            _unitOfWork.Save();
    //        if (IsValid && canCommit)
    //            transaction.Commit();
    //    }
    //    catch (DbUpdateException e)
    //    {
    //        AddError(e.Message + " - " + e.InnerException);
    //        if (canCommit)
    //            transaction.Rollback();
    //    }
    //    finally
    //    {
    //        if (canCommit)
    //            transaction.Dispose();
    //    }
    //}

    private HaveId<long> UpdateStep(UpdateStepArbitrationCourtApplication dto, Action<ArbitrationCourtApplication> validation, bool applyFilter = true)
    {
        var canCommit = _unitOfWork.CurrentTransaction == null;
        var transaction = canCommit ? _unitOfWork.BeginTransaction()
        : null;

        validation += Validation(dto);

        try
        {
            var entity = Repository.UpdateStep(dto, validation, applyFilter);
            CombineStatuses(Repository);
            if (HasErrors)
                throw new();

            _unitOfWork.Save();
            if (IsValid && canCommit)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            if (canCommit)
                transaction.Rollback();
        }

        return null;
    }

    private Action<ArbitrationCourtApplication> Validation(UpdateStepArbitrationCourtApplication dto)
    {
        return ent =>
        {
            if (ent.Application?.CurrentStepId == null ||
            (!StepIdConst.CanArbitrationCourtApplicationApplyStep(ent.Application.CurrentStepId.Value, dto.CurrentStepId)))
                AddError("Имкони йўқ / Нет доступа");
        };
    }
    #endregion


    #region Files
    public IEnumerable<ArbitrationCourtApplicationFileDto> UploadFiles(params StorageFile[] files)
    {
        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_ARBITRATION_APPLICATION, files)
            .Select(a => new ArbitrationCourtApplicationFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                IsCreatedErp = _authService.Contractor == null,
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }
    public StorageFile DownloadFile(Guid fileId)
    {
        var entity = _unitOfWork.Context.Set<ArbitrationCourtApplicationFile>().FirstOrDefault(a => a.Id == fileId);
        StorageFile file;
        if (entity.CanSign)
        {
            file = new(
                fileId: fileId,
                fileName: entity.FileName,
                new MemoryStream(DownloadPdf(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_APPLICATION).Result));
        }
        else
            file = Download(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_APPLICATION);

        return file;
    }
    public void DeleteFile(Guid fileId)
    {
        var entity = _unitOfWork.Context
            .Set<ArbitrationCourtApplicationFile>()
            .FirstOrDefault(a => a.Id == fileId);

        Delete(fileId, entity, DocumentStorageConst.DOC_ARBITRATION_APPLICATION);
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
    private async Task<byte[]> DownloadPdf(Guid fileId, FileEntity<long> entity, string storageDocument)
    {
        var storageFile = Download(fileId, entity, storageDocument);
        var wordFile = storageFile.GetStream();
        var doc = Repository.AllAsQueryable
            .FirstOrDefault(x => x.Files.Any(f => f.Id == fileId));
        if (doc == null)
        {
            AddError("Hujjat topilmadi.");
            return null;
        }
        var plh = WordFactory.MakePlaceholders(doc);
        #region QrCodes

        var signSsp = UnitOfWork.Context
          .Set<ArbitrationCourtApplicationSigner>()
          .FirstOrDefault(s => s.OwnerId == doc.Id);
        var signContractor = UnitOfWork.Context
            .Set<AdditionalAgreementSign>()
        .FirstOrDefault(s =>
        s.OwnerId == doc.Id
            && s.StatusId == StatusIdConst.SIGNED);


        //var link = _systemConf.QrImagePrintMy + "/Arbitration/ArbitrationCourtApplication/DownloadPdf?fileId=" + storageFile.FileId.ToString();

        //var qrDownload = new MemoryStream(QRCodeHelper.GeneratePng(link));
        //plh.ImagePlaceholders.Add("QrDownload", new() { Dpi = 512, MemStream = qrDownload });

        if (signSsp != null)
        {
            var QrCodeSsp = new MemoryStream(QRCodeHelper.GeneratePng(doc.Id.ToString()
                    + "  " + signSsp.SignedUserInfo));
            plh.ImagePlaceholders.Add("QrCodeSsp",
                new() { Dpi = 512, MemStream = QrCodeSsp });
            plh.TextPlaceholders.Add("QrCodeSsp", "++QrCodeSsp++");
        }
        else
            plh.TextPlaceholders.Add("QrCodeSsp", "");
        if (signContractor != null)
        {
            var QrCodeContractor = new MemoryStream(QRCodeHelper.GeneratePng(doc.Id.ToString()
                   + "  " + signContractor.SignedUserInfo));

            plh.ImagePlaceholders.Add("QrCode",
                new() { Dpi = 512, MemStream = QrCodeContractor });
            plh.TextPlaceholders.Add("QrCode", "++QrCode++");
        }
        else
            plh.TextPlaceholders.Add("QrCode", "");
        #endregion

        wordFile = (new DocXHandler((MemoryStream)wordFile, plh)).ReplaceAll();
        var res = await _pdfConverter.DocxToPdfAsync((MemoryStream)wordFile, new());

        return res;
    }

    public async Task<byte[]> DownloadTemplate(string? lang)
    {
        var langueage = lang ?? "uz-latn";
        try
        {
            var wordFile = _storageService.GetStaticFile(
                StaticFileConst.WordTemplate.GetFileName(
                   langueage,
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
    #endregion

    #region WebImzo
    public async ValueTask<string> WebImzoSign(WebImzoSignedFilter filter)
    {

        using var transaction = _unitOfWork.BeginTransaction();
            
        var entity = Repository.AllAsQueryable
            .Include(x => x.Signer)
            .ThenInclude(x => x.ArbitrationJudge)
            .FirstOrDefault(x => x.Id == filter.Id);

        if (entity == null)
        {
            AddError("Not found");
            transaction.Rollback();
            return null;
        }

        ArbitrationCourtApplicationSigner signer = entity.Signer
                                                            .FirstOrDefault(x => x.ArbitrationJudge.PersonId == _authService.User.PersonId
                                                             && x.SignedAt == null);
        if(signer == null)
        {
            if (entity.Signer.Any(x => x.EmployeeManageId != null))
            {
                AddError("Bu hujjat imzolangan");
            }

            else
            {
                if (_authService.User.EmployeeManageId == null)
                {
                    AddError("Hodimlar boshqaruvidan sizni topa olmadik.  ;(");
                }
                var employeeManage = _unitOfWork.EmployeeManageRepository.AllAsQueryable
                .FirstOrDefault(x => x.Id == _authService.User.EmployeeManageId.Value
                && x.EndOn == null && x.IsDeleted == false);

                if (employeeManage == null)
                {
                    AddError("Hodimlar boshqaruvidan sizni topa olmadik.  ;(");
                }
                if (entity.Signer.Any(x => x.SignedAt == null))
                {
                    AddError("Bu hujjatda imzolar to'liq emas");
                }
                signer = new()
                {
                    SignOrder = entity.Signer.Max(x => x.SignOrder) + 1,
                    DepartmentId = employeeManage.DepartmentId,
                    PositionId = employeeManage.PositionId,
                    EmployeeManageId = employeeManage.Id
                };

                entity.Signer.Add(signer);
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }
            }

        }

        return await PostToIMZOAndSentUrl(entity);
    }
    #endregion
    public async ValueTask<string> PostToIMZOAndSentUrl(ArbitrationCourtApplication application)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(application);
        signRequestCreateDto.SignRequestUsers = new List<WbImzoCreateSignRequestUserDto>
                                                    {
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = _authService.User.Pinfl,
                                                            UserInfo = _authService.User.FullName,
                                                            UserId = _authService.User.Id,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = 1,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                        }
                                                    };

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Ariza imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
            return null;
        }

        try
        {
            application.WebImzoRequestId = wbImzoResult.Response.RequestId;
            application.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(application.Id);
    }

    private async ValueTask<string?> SendUrl(long contractId)
    {

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        var contract = _unitOfWork.Context.Set<ServiceContract>().FirstOrDefault(a => a.Id == contractId);

        if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
        {
            AddError("Ariza imzolash uchun yuborilayotgan jarayonda qaytgan keylani saqlashda xatolik yuz berdi");
            return null;
        }

        var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

        return url;

    }

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(ArbitrationCourtApplication contract)
    {
        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}/{contract.Files}?";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.ECourtNumber,
            IsForceCreate = true,
            TableId = TableIdConst.DOC_SERVICE_CONTRACT,
            SignData = documentDataAsString,
           // OrganizationInn = organization != null ? organization.Inn : null,
           // OrganizationName = organization != null ? organization.FullName : null,
            PrintableLink = filePrintableLink,
            SignRequestActionTypes = new()
            {
                new WbImzoCreateSignRequestActionTypeDto
                {
                    ActionTypeId =  StatusIdConst.SIGNED,
                    ActionTypeName = "SIGNED",
                        Translates = new()
                        {
                            new ActionTranslateTypeDto
                            {
                                LanguageCode = LanguageCodeConst.RU,
                                Name =  "Я согласен"
                            },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_LATN,
                                Name = "Roziman"
                           },
                           new ActionTranslateTypeDto
                           {
                                LanguageCode = LanguageCodeConst.UZ_CYRL,
                                Name = "Розиман"
                           },
                        }
                },
            },
            SignatureMethodIds = new List<int> { SignatureMethodIdConst.E_IMZO },
            TemplateId = 28
        };
    }
    public WebImzoDto ConvertToDto(ArbitrationCourtApplication dto)
    => new()
    {
        DocOn = dto.CreatedAt.AsDateOnly(),
        DocNumber = dto.ECourtNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };
}