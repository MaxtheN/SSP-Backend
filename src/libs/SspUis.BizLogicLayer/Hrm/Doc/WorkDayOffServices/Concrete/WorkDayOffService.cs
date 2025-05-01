using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public class WorkDayOffService
    : BaseEntityService<long, WorkDayOff, WorkDayOffListDto, WorkDayOffDto, CreateWorkDayOffDlDto, UpdateWorkDayOffDlDto, IWorkDayOffRepository, WorkDayOffSortFilterOptions>
    , IWorkDayOffService
{

    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly INumberService _numberService;
    private readonly IWorkDayOffRepository _repository;
    private readonly IAuthService _authService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEImzoService _eImzoService;
    private readonly IStorageService _storageService;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;

    public WorkDayOffService(IUnitOfWork unitOfWork, 
                             IStorageService storageService, 
                             IEImzoService eImzoService, 
                             INumberService numberService, 
                             IAuthService authService,
                             WbImzoConfig wbImzoConfig,
                             List<LinkConfig> linkConfigs,
                             IWbImzoService wbImzoService,
                             IDocumentChangeLogService documentChangeLogService) : base(unitOfWork)
    {
        this._repository = unitOfWork.WorkDayOffRepository;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
        this._unitOfWork = unitOfWork;
        _eImzoService = eImzoService;
        _authService = authService;
        _storageService = storageService;
        _wbImzoService = wbImzoService;
        _wbImzoConfig = wbImzoConfig;
        _linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "WorkDayOff");
    }
    public PagedResult<WorkDayOffListDto> GetList(WorkDayOffSortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<WorkDayOffListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }
    public SelectList<long> AsSelectList(WorkDayOffSortFilterOptions options)
    {
        return Repository.ReadAsNoTracked<WorkDayOffListDto>()
            .SortFilter(options)
            .AsSelectList();
    }
    public WorkDayOffDto Get()
    {
        return new WorkDayOffDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_WORK_DAY_OFF, 1).Item2
        };
    }
    public WorkDayOffDto Get(long id)
    {
        var dto = _repository.ById<WorkDayOffDto>(id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            var nextSigner = _unitOfWork.Context.Set<WorkDayOffSigner>()
                    .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                    .OrderBy(a => a.SignOrder).FirstOrDefault();

            dto.CanSign = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.WorkDayOffSign) && (nextSigner != null && nextSigner.EmployeeManageId == _authService.User.EmployeeManageId);
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.WorkDayOffEdit);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.WorkDayOffCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.WorkDayOffDelete);
        }
        return dto;
    }
    public async ValueTask<HaveId<long>> Create(CreateWorkDayOffDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "CreateWorkDayOff");
                CombineStatuses(Repository);
                if (IsValid)
                {
                    transaction.Commit();

                    //var model = _unitOfWork.Context.Set<WorkDayOff>()
                    //                                        .Include(a => a.Signer)
                    //                                            .ThenInclude(a => a.EmployeeManage)
                    //                                                .ThenInclude(em => em.Employee)
                    //                                                    .ThenInclude(e => e.Organization)
                    //                                        .Include(a => a.Signer)
                    //                                            .ThenInclude(a => a.EmployeeManage)
                    //                                                .ThenInclude(em => em.Employee)
                    //                                                    .ThenInclude(e => e.Person)
                    //                                        .FirstOrDefault(a => a.Id == entity.Id);

                    //await PostToIMZOAndSentUrl(model);

                }
                return HaveId.Create(entity.Id);
            }
            catch(DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
            return null;
        }
    }

    public async ValueTask<string> PostToIMZOAndSentUrl(WorkDayOff contract)
    {
        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        WbImzoCreateSignRequestDto signRequestCreateDto = CreatDtoForRequestSign(contract);


        foreach (var signer in contract.Signer)
        {

            if (contract.Signer.Count == signer.SignOrder)
            {

                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Organization.Inn) ? signer.EmployeeManage.Employee.Organization.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNED,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.Person.PassportNumber,
                                                        }
                                                    );
            }

            else
            {
                signRequestCreateDto.SignRequestUsers.Add(
                                                        new WbImzoCreateSignRequestUserDto
                                                        {
                                                            UserKey = !string.IsNullOrEmpty(signer.EmployeeManage.Employee.Person.Inn) ? signer.EmployeeManage.Employee.Person.Inn : signer.EmployeeManage.Employee.Person.Pinfl,
                                                            UserInfo = signer.EmployeeManage.Employee.Person.FullName,
                                                            UserId = (int)contract.CreatedUserId,
                                                            DocStatusId = StatusIdConst.SIGNING,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.Person.PassportNumber,
                                                        }
                                                    );
            }
        }

        if (contract.WebImzoSecretKey != null)
            return await SendUrl(contract.Id);

        var wbImzoResult = await _wbImzoService.UpsertStringFilePrintableLinkRequestAsync(signRequestCreateDto);
        if (!wbImzoResult.IsSuccess && wbImzoResult.Response is null)
            CombineStatuses(wbImzoResult.GetStatusGeneric());

        if (HasErrors || wbImzoResult.Response is null)
        {
            AddError("Bir martalik tolov berish buyuruqni imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
            return null;
        }

        try
        {
            contract.WebImzoRequestId = wbImzoResult.Response.RequestId;
            contract.WebImzoSecretKey = wbImzoResult.Response.SecretKey;

            _unitOfWork.Save();
            CombineStatuses(this);
            if (canDispose)
                transaction.Commit();
        }
        catch (Exception ex)
        {
            AddError("Document did not update" + ex.Message);
        }

        return await SendUrl(contract.Id);
    }
    public async ValueTask<string?> SendUrl(long contractId)
    {

        var contract = _unitOfWork.Context.Set<WorkDayOff>()
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Organization)
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Person)
                                                          .FirstOrDefault(a => a.Id == contractId);

        if (contract == null)
        {
            AddError("Ma'lumot topilmadi");
            return null;
        }

        if (contract.Signer.Count() > 0)
        {
            var nextSigner = _unitOfWork.Context.Set<WorkDayOffSigner>()
                                                                 .Where(a => a.OwnerId == contractId)
                                                                 .OrderBy(a => a.SignOrder);
            if (nextSigner == null)
                AddError("Imzolovchi topilmadi");
            else if (!nextSigner.Any(a => a.EmployeeManageId == _authService.User.EmployeeManageId))
                AddError("Sizda imzolash huquqi yo'q");
            if (HasErrors)
                return null;

            if (contract.WebImzoRequestId == null || contract.WebImzoSecretKey.IsNullOrEmpty())
            {
                await PostToIMZOAndSentUrl(contract);
            }

            var url = (_wbImzoConfig.Api.Contains("api/") ? _wbImzoConfig.Api.Replace("api/", null) : _wbImzoConfig.Api) + $"app?requestId={contract.WebImzoRequestId}&secretKey={contract.WebImzoSecretKey}";

            return url;
        }

        return null;
    }
    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(WorkDayOff contract)
    {
        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
       // var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = false,
            TableId = TableIdConst.DOC_WORK_DAY_OFF,
            SignData = documentDataAsString,
            OrganizationInn = contract.Organization.Inn,
            OrganizationName = contract.Organization.FullName,
           // PrintableLink = filePrintableLink,
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
    public WebImzoDto ConvertToDto(WorkDayOff dto)
    => new()
    {
        DocOn = dto.DocOn,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };

    public override void Update(UpdateWorkDayOffDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "UpdateWorkDayOff");
                CombineStatuses(Repository);
                if(IsValid)
                    transaction.Commit();
            }
            catch(DbUpdateException e)
            {
                AddError(e.Message);
                transaction.Rollback();
            }
        }
    }
    public override void Delete(long id)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusWorkDayOffDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                        Repository.AddError("Нет доступа");
                });
                Repository.UpdateStatus(statusDto);
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteWorkDayOff");

                if(IsValid)
                {
                    transaction.Commit();
                }
            }
            catch(DbUpdateException ex)
            {
                AddError(ex.Message);
                transaction.Rollback();
            }
        }
    }
    public void Accept(UpdateStatusWorkDayOffDto dTo, bool isLoged = false)
    {
        try
        {
            var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
            var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;

            if (isLoged)
            {
                Repository.AllAsQueryable.Lock(dTo.Id);
            }

            var dto = new UpdateStatusWorkDayOffDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };

            UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                {
                    _repository.AddError("Нет доступа");
                }

                bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
                    .Where(a => ent.Tables.Select(a => a.EmployeeManageId).Contains(a.Id))
                    .Any(a => a.IsDeleted);

                if (isDeleted)
                {
                    _repository.AddError("Сотрудник был удален");
                }
            });
            if (!IsValid)
            {
                transaction.Rollback();
                return;
            }

            if (canCommit)
            {
                transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
        }
    }
    public void Cancel(UpdateStatusWorkDayOffDto dTo)
    {
        var dto = new UpdateStatusWorkDayOffDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                _repository.AddError("Нет доступа");
            var res = CreateDocumentChangeLog(dto.Id, dto.StatusId);
            //_hrmPackageContext.DocControlPackage.EmployeeLeaveOrderCancelValidation(
            //    organizationId: _authService.CurrentOrganizationId,
            //    id: dto.Id,
            //    userId: (int)_authService.UserId
            //);
            CombineStatuses(_repository);
        });
    }
    public async Task Sign(SignStatusWorkDayOffDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.ById(dto.Id);

                //if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) || entity.Signer.Count() == 0)
                //{
                //    AddError("Siz Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
                //}
                //if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) || (entity.Signer.Count() == 0 && !_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner)))
                //{
                //    AddError("Siz Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
                //}
                if (HasErrors)
                {
                    transaction.Rollback();
                    return;
                };

                
                    var nextSigner = _unitOfWork.Context.Set<WorkDayOffSigner>()
                    .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                    .OrderBy(a => a.SignOrder).FirstOrDefault();
                    if (nextSigner == null)
                        AddError("Imzolovchi topilmadi");
                    else if (nextSigner.EmployeeManageId != _authService.User.EmployeeManageId)
                        AddError("Sizda imzolash huquqi yo'q");
                    if (HasErrors)
                        return;


                    var eImzoTimstampDto = new EImzoTimeStampDto
                    {
                        SignData = dto.SignedData,
                        Inn = null,
                        Pinfl = _authService.User.Pinfl
                    };

                    var timeStamp = _eImzoService.TimeStamp(eImzoTimstampDto).Result;

                    CombineStatuses(_eImzoService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    };

                    var eImzoVerifyAttached = _eImzoService.VerifyAttached(new EImzoVerifyDto
                    {
                        SignData = timeStamp.Pkcs7b64,
                        Inn = null,
                        Pinfl = _authService.User.Pinfl
                    }).Result;

                    nextSigner.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
                    nextSigner.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
                    nextSigner.SignedAt = DateTime.Now;

                    _unitOfWork.Context.Entry(nextSigner).State = EntityState.Modified;
                    _unitOfWork.Save();

                    //var dto = new UpdateStatusAppointEmployeeDlDto { Id = dto.Id, StatusId = StatusIdConst.SIGNING };

                    var ent = _repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplyEmployeeLeaveOrder(ent.StatusId, dto.StatusId))
                            _repository.AddError("Нет доступа");
                    });
                    CombineStatuses(_repository);
                    if (HasErrors)
                        return;

                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.SIGNING);
                        if (nextSigner.IsDirector)
                        {
                            Accept(new UpdateStatusWorkDayOffDto
                            {
                                Id = dto.Id,
                                StatusId = StatusIdConst.ACCEPTED
                            });
                        }
                        transaction.Commit();
                    }
            }
            catch (DbUpdateException)
            {
                AddError("Запись не может быть удален");
                transaction.Rollback();
            }
        }
    }
    private HaveId<long> UpdateStatus(UpdateStatusWorkDayOffDlDto dto, Action<WorkDayOff> validation)
    {
        using (var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.UpdateStatus(dto, validation);
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
                if (IsValid)
                    transaction.Commit();
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
    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = Repository.ById<WorkDayOffDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_WORK_DAY_OFF,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if(HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(WorkDayOffDlDto<TDto> dto, WorkDayOff entity)
       where TDto : WorkDayOffDlDto<TDto>
    {
        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

        if (dto.Signer.Count() > 0)
        {
            if (dto.Signer.Count(a => a.IsHr) == 0)
                AddError("Имзоловчи кадр киритилмаган");
            if (dto.Signer.Count(a => a.IsHr) > 1)
                AddError("Имзоловчи кадр лавозимидаги ходим 1 та болиши керак");
            if (dto.Signer.Count(a => a.IsDirector) == 0)
                AddError("Имзоловчи директор киритилмаган");
            if (dto.Signer.Count(a => a.IsDirector) > 1)
                AddError("Имзоловчи дтректор лавозимидаги ходим 1 та болиши керак");
        }

    }
    public Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_WORK_DAY_OFF)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }
}
