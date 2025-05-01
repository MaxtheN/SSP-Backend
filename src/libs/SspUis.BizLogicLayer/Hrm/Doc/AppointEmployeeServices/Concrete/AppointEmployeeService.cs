using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OpenXmlPowerTools;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using SspUis.BizLogicLayer.IntegrationServices.Xodim;
using SspUis.BizLogicLayer.Srv;
using SspUis.BizLogicLayer.UserServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.Integration.DocxToPdf;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WbAccessControl.Sdk;
using WbImzo.Models;
using WbImzo.Proxy.Sdk;
using WEBASE;
using WEBASE.Integration.EImzo;
using WEBASE.Models;
using WEBASE.OfficeTools.Factory;
using WEBASE.OfficeTools.Handlers;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Hrm;

public partial class AppointEmployeeService : BaseEntityService<long, AppointEmployee, AppointEmployeeListDto, AppointEmployeeDto, CreateAppointEmployeeDlDto, UpdateAppointEmployeeDlDto, IAppointEmployeeRepository, AppointEmployeeSortFilterOptions>
    , IAppointEmployeeService
{
    private readonly IAppointEmployeeRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly IDocumentChangeLogService _documentChangeLogService;
    private readonly IEmployeeManageRepository _employeeManageRepository;
    private readonly INumberService _numberService;
    private readonly IConvertService _pdfConverter;
    private readonly IUserService _userService;
    private readonly IDocumentPrintService _print;
    private readonly IEImzoService _eImzoService;
    private readonly IStorageService _storageService;
    private readonly IWbacClientService _wbacClientService;
    private readonly IDocumentChangeLogRepository _documentChangeLogRepository;
    private readonly IWbImzoService _wbImzoService;
    private readonly WbImzoConfig _wbImzoConfig;
    private readonly LinkConfig _linkConfig;
    private readonly IXodimPhotoUploader _photoUploader;

	public AppointEmployeeService(
		IUnitOfWork unitOfWork,
		IAuthService authService,
		INumberService numberService,
		IEImzoService eImzoService,
		IEmployeeManageRepository employeeManageRepository,
		IUserService userService,
		IStorageService storageService,
		IDocumentChangeLogService documentChangeLogService,
		IConvertService pdfConverter,
		IDocumentPrintService print,
		IWbacClientService wbacClientService,
		WbImzoConfig wbImzoConfig,
		List<LinkConfig> linkConfigs,
		IWbImzoService wbImzoService,
		IDocumentChangeLogRepository documentChangeLogRepository,
		IXodimPhotoUploader photoUploader) : base(unitOfWork)
	{
		this._repository = unitOfWork.AppointEmployeeRepository;
		this._unitOfWork = unitOfWork;
		this._authService = authService;
		this._documentChangeLogService = documentChangeLogService;
		this._employeeManageRepository = employeeManageRepository;
		this._numberService = numberService;
		this._pdfConverter = pdfConverter;
		this._userService = userService;
		this._print = print;
		_eImzoService = eImzoService;
		_storageService = storageService;
		_wbacClientService = wbacClientService;
		this._documentChangeLogRepository = documentChangeLogRepository;
		_wbImzoService = wbImzoService;
		_wbImzoConfig = wbImzoConfig;
		_linkConfig = linkConfigs.FirstOrDefault(config => config.Name == "AppointEmployee");
		_photoUploader = photoUploader;
	}

	public PagedResult<AppointEmployeeListDto> GetList(AppointEmployeeSortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<AppointEmployeeListDto>()
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }

    public PagedResult<AppointEmployeeListDto> GetListForHeader(AppointEmployeeSortFilterOptions dto)
    {
        var result = _repository.ReadAsNoTracked<AppointEmployeeListDto>().Where(a => new int[]
        {
                    StatusIdConst.ACCEPTED,StatusIdConst.CREATED,
                    StatusIdConst.MODIFIED, StatusIdConst.NOT_ACCEPTED,
                    StatusIdConst.SIGNED, StatusIdConst.SIGNING
        }.Contains(a.StatusId))
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }

    public PagedResult<AppointEmployeeListDto> GetListForSigner(AppointEmployeeSortFilterOptions dto)
    {

        var result = _repository.ReadAsNoTracked<AppointEmployeeListDto>(q =>
                q.Signer.Any(a => a.EmployeeManageId == _authService.User.EmployeeManageId)
            && (new int[] { StatusIdConst.ACCEPTED, StatusIdConst.SIGNED, StatusIdConst.SIGNING }).Contains(q.StatusId))
                                .SortFilter(dto)
                                .AsPagedResult(dto);
        return result;
    }

    public AppointEmployeeDto Get()
    {
        return new AppointEmployeeDto()
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_APPOINT_EMPLOYEE, 1).Item2
        };
    }

    public AppointEmployeeDto Get(long id)
    {
        var dto = _repository.ById<AppointEmployeeDto>(id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            var nextSigner = _unitOfWork.Context.Set<AppointEmployeeSigner>()
                    .Where(a => a.OwnerId == dto.Id && !a.SignedAt.HasValue)
                    .OrderBy(a => a.SignOrder).FirstOrDefault();

            dto.CanSign = StatusIdConst.CanApplyAppointEmployee(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.AppointEmployeeSign) && (nextSigner == null || (nextSigner != null && nextSigner.EmployeeManageId == _authService.User.EmployeeManageId));
            dto.CanModify = StatusIdConst.CanApplyAppointEmployee(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.AppointEmployeeEdit);
            dto.CanAccept = StatusIdConst.CanApplyAppointEmployee(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.AppointEmployeeAccept);
            dto.CanCancel = StatusIdConst.CanApplyAppointEmployee(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.AppointEmployeeCancel);
            dto.CanDelete = StatusIdConst.CanApplyAppointEmployee(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.AppointEmployeeDelete);
        }
        return dto;
    }

    public SelectList<long> AsSelectList(int? employeeId = null)
    {
        return _repository.AllAsQueryable
            .Include(a => a.Tables)
            .Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }

    public async ValueTask<HaveId<long>> Create(CreateAppointEmployeeDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _repository.Create(dto, ent =>
            {
                foreach (var table in dto.Tables)
                {
                    if (table.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL)
                    {
                        if (!table.DepartmentId.HasValue | !table.EmploymentTypeId.HasValue | !table.PositionId.HasValue)
                            _repository.AddError("Недостаточно данных");
                    }
                }
                Validation(dto, ent);
            });

            CombineStatuses(_repository);
            if (HasErrors)
                return null;
            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
            {
                transaction.Commit();

                //var model = _unitOfWork.Context.Set<AppointEmployee>()
                //                                                  .Include(a => a.Signer)
                //                                                      .ThenInclude(a => a.EmployeeManage)
                //                                                          .ThenInclude(em => em.Employee)
                //                                                              .ThenInclude(e => e.Organization)
                //                                                  .Include(a => a.Signer)
                //                                                      .ThenInclude(a => a.EmployeeManage)
                //                                                          .ThenInclude(em => em.Employee)
                //                                                              .ThenInclude(e => e.Person)
                //                                                  .FirstOrDefault(a => a.Id == entity.Id);

                //await PostToIMZOAndSentUrl(model);
                //_numberService.Save(TableIdConst.DOC_APPOINT_EMPLOYEE);
            }
            return res;
        }
    }

    public async ValueTask<string> PostToIMZOAndSentUrl(AppointEmployee contract)
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
                                                            DocStatusId =  StatusIdConst.SIGNED,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.PhoneNumber
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
                                                            DocStatusId =  StatusIdConst.SIGNING,
                                                            SignPriority = signer.SignOrder,
                                                            IpAddress = _authService.UserIp,
                                                            UserAgent = _authService.UserAgent,
                                                            UserPhoneNumber = signer.EmployeeManage.Employee.PhoneNumber,
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
            AddError("Contract imzolash uchun yuborilayotgan jarayonda xatolik yuz berdi");
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

        var contract = _unitOfWork.Context.Set<AppointEmployee>()
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Organization) 
                                                          .Include(a => a.Signer)
                                                              .ThenInclude(a => a.EmployeeManage)
                                                                  .ThenInclude(em => em.Employee)
                                                                      .ThenInclude(e => e.Person) 
                                                          .FirstOrDefault(a => a.Id == contractId);


        var entity = _repository.ById(contractId);

        if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) && entity.Signer.Count() == 0)
        {
            _repository.AddError("Sizda Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
            CombineStatuses(_repository);

            if (HasErrors)
            {
                return null;
            }
        }
        if (entity.Signer.Count() > 0)
        {
            var nextSigner = _unitOfWork.Context.Set<AppointEmployeeSigner>()
                                                                 .Where(a => a.OwnerId == contractId)
                                                                 .OrderBy(a => a.SignOrder);
            if (nextSigner == null)
                AddError("Imzolovchi topilmadi");
            else if (!nextSigner.Any(a => a.EmployeeManageId != _authService.User.EmployeeManageId))
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

    public WbImzoCreateSignRequestDto CreatDtoForRequestSign(AppointEmployee contract)
    {
    

        string documentDataAsString = JsonConvert.SerializeObject(ConvertToDto(contract));
        var filePrintableLink = $"{_linkConfig.Api}{_linkConfig.DownloadRoute}?id2={contract.Id2}&lang=uz-cyrl&__lang=uz-cyrl&langId=2";

        return new WbImzoCreateSignRequestDto
        {
            DocumentId = contract.Id,
            ApiKey = _wbImzoConfig.ApiKey,
            Title = contract.DocNumber,
            IsForceCreate = false,
            TableId = TableIdConst.DOC_APPOINT_EMPLOYEE,
            SignData = documentDataAsString,
            //OrganizationInn = organization != null ? organization.Inn : null,
            //OrganizationName = organization != null ? organization.FullName : null,
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
    public WebImzoDto ConvertToDto(AppointEmployee dto)
    => new()
    {
        DocOn = dto.DocOn,
        StatusId = dto.StatusId,
        DocNumber = dto.DocNumber,
        Id = dto.Id,
        OrganizationId = dto.OrganizationId,
    };

    public void Update(UpdateAppointEmployeeDlDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _repository.Update(dto, ent =>
            {
                if (!StatusIdConst.CanApplyAppointEmployee(ent.StatusId, StatusIdConst.MODIFIED))
                    _repository.AddError("Нет доступа");
                else
                    Validation(dto, ent);
            });
            CombineStatuses(_repository);
            if (HasErrors)
                return;

            // Agar TABLE qismidan nimadirri ocirgan bolishsa,
            // EmployeeManage tablitsadanam ocirib tashash kerak bo'ladi
            var deletedEmployeeManageIds = entity.Tables.Select(a => a.EmployeeManageId)
                .Except(dto.Tables.Select(a => a.EmployeeManageId));
            foreach (var employeeManageId in deletedEmployeeManageIds)
            {
                _employeeManageRepository.Delete(employeeManageId.Value);
            }

            _unitOfWork.Save();

            var res = CreateDocumentChangeLog(entity.Id, entity.StatusId);
            if (IsValid)
                transaction.Commit();
        }
    }

    public void Delete(long id)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                AppointEmployee entity = _repository.ById(id, applyFilter: false);
                if (entity.StatusId == StatusIdConst.ACCEPTED)
                {
                    AddError($"Ишдан кетиш санаси ишга қабул қилинган санадан катта бўлиши керак.");
                    return;
                }

                var dto = new UpdateStatusAppointEmployeeDlDto { Id = id, StatusId = StatusIdConst.DELETED };

                var ent = _repository.UpdateStatus(dto, ent =>
                {
                    if (!StatusIdConst.CanApplyAppointEmployee(ent.StatusId, dto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                CombineStatuses(_repository);
                _unitOfWork.Save();
                if (HasErrors)
                    return;

                var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.DELETED, "DeleteAppointEmployee");
                if (IsValid)
                {
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

    private void CheckEmploymentRate(AppointEmployeeTableDto table)
    {
        IQueryable<EmployeeManageDto> employeeManages = _employeeManageRepository.CrudServices
                                    .ProjectFromEntityToDto<EmployeeManage, EmployeeManageDto>(a => a.Where(b => b.EmployeeId == table.EmployeeId && b.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL && !b.IsDeleted && (b.EndOn == null || b.EndOn > table.StartOn)));

        if (table.FromPositionId > 0)
            employeeManages = employeeManages.Where(a => a.PositionId != table.FromPositionId);

        if (employeeManages.Any())
        {
            var totalRate = employeeManages.Sum(a => a.EmploymentRate) + table.EmployeeRate;

            if (totalRate > 1.5m)
            {
                var employmentRatesInfo = string.Join(", ", employeeManages.Select(a => $"{a.Organization} (ставка: {a.EmploymentRate}"));
                AddError($"{table.EmployeeFull} нинг жами ставкаси 1.5 дан ошиб кетди! жами ставка {totalRate}; {employmentRatesInfo}");
            }
        }
        else
        {
            if (table.EmployeeRate > 1.5m)
                AddError($"{table.EmployeeFull} нинг жами ставкаси 1.5 дан ошиб кетди! жами ставка {table.EmployeeRate};");
        }
    }

    public void CheckEmploymentRateBeforSave(int employeeId, DateTime startOn, decimal employeeRate, int? fromPositionId)
    {

        var employeeManages = _employeeManageRepository.CrudServices
                                .ProjectFromEntityToDto<EmployeeManage, EmployeeManageDto>(a => a.Where(b => b.EmployeeId == employeeId && b.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL && !b.IsDeleted && (b.EndOn == null || b.EndOn > startOn.AsDateOnly())));

        var employeeName = _unitOfWork.Context.Set<Employee>().Include(a => a.Person).FirstOrDefault(a => a.Id == employeeId).Person.FullName;

        if (fromPositionId > 0)
            employeeManages = employeeManages.Where(a => a.PositionId != fromPositionId);

        if (employeeManages.Any())
        {
            var totalRate = employeeManages.Sum(a => a.EmploymentRate) + employeeRate;

            if (totalRate > 1.5m)
            {
                var employmentRatesInfo = string.Join(", ", employeeManages.Select(a => $"{a.Organization} (ставка: {a.EmploymentRate}"));
                AddError($"{employeeName} нинг жами ставкаси 1.5 дан ошиб кетди! жами ставка {totalRate}; {employmentRatesInfo}");
            }
        }
        else
        {
            if (employeeRate > 1.5m)
                AddError($"{employeeName} нинг жами ставкаси 1.5 дан ошиб кетди! жами ставка {employeeRate};");
        }
    }

    private HaveId<long> UpdateStatus(UpdateStatusAppointEmployeeDlDto dto, Action<AppointEmployee> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = _repository.UpdateStatus(dto, validation);

                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, entity.StatusId, "AppointEmployee");
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

    public async Task<CreateUserResponseModel> AcceptAsync(UpdateStatusAppointEmployeeDto dto, bool isLoged = false)
    {
        var canCommit = _unitOfWork.Context.Database.CurrentTransaction == null;
        var transaction = canCommit ? _unitOfWork.BeginTransaction() : _unitOfWork.CurrentTransaction;
        if (isLoged)
            Repository.AllAsQueryable.Lock(dto.Id);

        //await _photoUploader.UploadPhotos();
        try
        {
            var entity = _repository.ById<AppointEmployeeDto>(dto.Id);
            var test = JsonConvert.SerializeObject(entity);

            if (entity.Tables.Any(a => a.PositionId == null && a.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL))
                AddError($"Ҳодимга лавозим бириктирилмаган !");

            #region Validation

            var noStaffingPosition = _unitOfWork.Context.Set<StaffingPosition>()
             .Where(a => a.Owner.OrganizationId == _authService.User.OrganizationId && a.Owner.StatusId != StatusIdConst.ARCHIVED).Select(a => a.PositionId).ToArray();

            var hireTable = entity.Tables.Where(x => x.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL);
            if (hireTable.Any())
            {
                Dictionary<string, decimal> staffings = _unitOfWork.Context.Set<Staffing>()
                  .Include(a => a.Positions)
                  .Where(a => a.OrganizationId == _authService.User.OrganizationId
                      && a.StatusId == StatusIdConst.ACCEPTED
                      && a.DocOn <= entity.DocOn
                  )
                  .SelectMany(a => a.Positions)
                  .Select(a => new
                  {
                      a.PositionClassificationId,
                      a.DepartmentId,
                      a.Quantity
                  })
                  .AsEnumerable()
                  .GroupBy(a => new { a.PositionClassificationId, a.DepartmentId })
                  .ToDictionary(
                      a => $"{a.Key.PositionClassificationId}-{a.Key.DepartmentId}",
                      a => a.Sum(b => b.Quantity)
                  );
                var temporaryTable = entity.Tables.Where(x => x.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                                                  .Select(x => x.FromEmployeeManageId);

                Dictionary<string, decimal> employeeManage = _unitOfWork.EmployeeManageRepository
             .AllAsQueryable
             .AsNoTracking()
                 .Where(a => a.EndOn == null)
                 .Include(a => a.Position)
                     .AsEnumerable()
                         .GroupBy(a => new
                         {
                             a.Position.PositionClassificationId,
                             a.DepartmentId
                         })
                             .ToDictionary(
                                 a => $"{a.Key.PositionClassificationId}-{a.Key.DepartmentId}",
                                 a => a.Sum(b => b.EmploymentRate.Value)
                             );

                foreach (var staffing in staffings)
                    if (employeeManage.ContainsKey(staffing.Key))
                        staffings[staffing.Key] -= employeeManage[staffing.Key];

                if (HasErrors)
                    return null;

                Dictionary<string, decimal> positions =
                    new Dictionary<string, decimal>();

                Dictionary<string, string> positionName =
                    new Dictionary<string, string>();

                foreach (var table in hireTable.GroupBy(a => new
                {
                    a.PositionId,
                    a.DepartmentId
                }))
                {
                    var keyPos = table.Key.PositionId.HasValue ? table.Key.PositionId.Value.ToString() : "";
                    string key = $"{keyPos}-{table.Key.DepartmentId}";

                    if (table.Key.PositionId.HasValue &&
                        !positions.ContainsKey(key))
                    {
                        positions.Add(key, table.Sum(a => a.EmployeeRate.Value));
                        positionName.Add(key, table.FirstOrDefault(x => x.PositionId == table.Key.PositionId.Value).Position);
                    }
                }

                foreach (var table in positions)
                {
                    string[] idPosition = table.Key.Split('-');
                    if (!noStaffingPosition.Contains(Int32.Parse(idPosition[0])))
                        if (!staffings.ContainsKey(table.Key))
                            AddError($"{positionName[table.Key]}  \nШтатда бундай лавозим классификацияси кўрсатилмаган.");
                        else if (staffings[table.Key] < table.Value)
                            AddError($"Штатда бундай лавозим классификацияси етарли эмас. Штатда ({staffings[table.Key]}-{positionName[table.Key]})");
                }

                if (HasErrors)
                {
                    if (canCommit)
                        transaction.Rollback();
                    return null;
                }
                // Ishdan bo'shatilvotgan yoki lavozimini o'zgartrvotgan hodimla IsDeleted bo'lishi kerak emas
                var fromEmployeeManageIds = entity.Tables.Where(b => b.FromEmployeeManageId.HasValue).Select(b => b.FromEmployeeManageId).ToList();
                var deletedEmployees = UnitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                    .Where(a => fromEmployeeManageIds.Contains(a.Id) && a.IsDeleted)
                    .Select(a => a.Employee.Person.FullName);
                if (deletedEmployees.Any())
                    AddError($"Қуйидаги ходимларни ишга қабул қилиш хужжати бекор қилинган: {string.Join(", ", deletedEmployees)}");

                // ishga qabul qilingan sanasi ishdan ketgan sanasidan kichkina bolishi kerak
                var employees = UnitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                    .Where(a => fromEmployeeManageIds.Contains(a.Id))
                    .Select(a => new { a.Employee.Person.FullName, a.StartOn });
                if (employees.Any(a => a.StartOn >= entity.DocOn))
                    AddError($"Ишдан кетиш санаси ишга қабул қилинган санадан катта бўлиши керак.");

                if (HasErrors)
                {
                    if (canCommit)
                        transaction.Rollback();
                    return null;
                }
            }
            #endregion

            #region All Order Type

            foreach (var table in entity.Tables)
            {
                if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE ||
                    table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                {
                    CheckEmploymentRate(table);

                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }

                    #region Chek Is Postion Booked
                    var isPostionBooked = _unitOfWork.EmployeeManageRepository.AllAsQueryable.FirstOrDefault(m => m.DepartmentId == table.DepartmentId && m.PositionId == table.PositionId && m.EmploymentRate == 0 && m.EndOn == null && !m.IsDeleted);

                    if (isPostionBooked != null)
                    {
                        //_employeeManageRepository.SetEnd(
                        //    id: isPostionBooked.Id,
                        //    endOn: table.StartOn,
                        //    endDocumentId: isPostionBooked.DocId,
                        //    TableIdConst.DOC_APPOINT_EMPLOYEE);
                        using (var command = _unitOfWork.Context.Database.GetDbConnection().CreateCommand())
                        {

                            command.CommandText = @$"UPDATE hrm.sys_employee_manage SET 
                                                            end_table_id = @end_table_id,
                                                            end_doc_id = @end_doc_id,
                                                            end_on = @end_on WHERE id = @id";

                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_table_id", TableIdConst.DOC_APPOINT_EMPLOYEE));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_doc_id", isPostionBooked.DocId));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_on", isPostionBooked.StartOn.AddDays(-1)));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("id", isPostionBooked.Id));

                            command.ExecuteNonQuery();
                        }
                    }
                    _unitOfWork.Save();
                    #endregion

                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }
                }
                var createEmployeeManageDlDto = new CreateEmployeeManageDlDto
                {
                    Id = table.EmployeeManageId.Value,
                    DocId = entity.Id,
                    DocTableId = TableIdConst.DOC_APPOINT_EMPLOYEE,
                    EmpAppointOrderTypeId = table.EmpAppointOrderTypeId,
                    StartOn = table.StartOn,
                    DepartmentId = table.DepartmentId.HasValue ? table.DepartmentId.Value : 0,
                    PositionId = table.PositionId.HasValue ? table.PositionId.Value : 0,
                    EmploymentTypeId = table.EmploymentTypeId.HasValue ? table.EmploymentTypeId.Value : 0,
                    EmploymentRate = table.EmployeeRate.HasValue ? table.EmployeeRate.Value : 0,
                    WorkScheduleId = table.WorkScheduleId.HasValue ? table.WorkScheduleId.Value : 0,
                    EmployeeId = table.EmployeeId,
                    EndByDocumentOn = table.EndOn,
                    OrganizationId = entity.OrganizationId.Value,
                };

                // IsDeleted ga filtr qoyish shart emas
                bool exists = _unitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                    .Any(a => a.Id == table.EmployeeManageId);

                if (createEmployeeManageDlDto.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE
                    || createEmployeeManageDlDto.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.COMBINING)
                {
                    if (exists)
                    {

                        var otherData = _unitOfWork.Context.Set<EmployeeManage>()
                         .Any(a => a.Id == table.EmployeeManageId && a.EmployeeId != table.EmployeeId);
                        if (otherData)
                        {
                            AddError("hujjat boshqa hodimga biriktirilgan");
                            return null;
                        }
                        var mc = new AutoMapper.MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>();
                        });
                        var updateDlDto = mc.CreateMapper().Map<UpdateEmployeeManageDlDto>(createEmployeeManageDlDto);
                        _employeeManageRepository.Update(updateDlDto);
                    }

                    else
                    {
                        _employeeManageRepository.Create(createEmployeeManageDlDto);
                    }
                    CombineStatuses(_employeeManageRepository);

                    #region For Turniket 
                    if (IsValid)
                    {
                        var entry = _unitOfWork.Context.Set<Employee>()
                            .Include(a => a.Organization)
                            .Include(a => a.Person)
                            .FirstOrDefault(a => a.Id == table.EmployeeId);

                        if (entry.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP || entry.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
                        {
                            var turnstilePersonDataResult = await _wbacClientService.UpSertPersonAsync(new WbacTurnstilePersonDto
                            {
                                PersonId = entry.Id,
                                TableId = TableIdConst.HL_EMPLOYEE,
                                OrganizationId = entry.OrganizationId,
                                FullName = entry.Person.FullName,
                                ImageId = entry.Person.PictureId != null ? entry.Person.PictureId.ToString() : null
                            });

                            if (!turnstilePersonDataResult.IsSuccess || turnstilePersonDataResult.Response)
                                CombineStatuses(turnstilePersonDataResult.GetStatusGeneric());
                        }
                    }
                    #endregion

                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }
                }
                else if (createEmployeeManageDlDto.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                {
                    if (!table.Interm)
                    {
                        using (var command = _unitOfWork.Context.Database.GetDbConnection().CreateCommand())
                        {

                            command.CommandText = @$"UPDATE hrm.sys_employee_manage SET 
                                                            end_table_id = @end_table_id,
                                                            end_doc_id = @end_doc_id,
                                                            end_on = @end_on WHERE id = @id";

                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_table_id", TableIdConst.DOC_APPOINT_EMPLOYEE));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_doc_id", createEmployeeManageDlDto.DocId));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("end_on", createEmployeeManageDlDto.StartOn.AddDays(-1)));
                            command.Parameters.Add(new Npgsql.NpgsqlParameter("id", table.FromEmployeeManageId.Value));

                            command.ExecuteNonQuery();
                        }
                    }
                    /*var entityToDetach = _employeeManageRepository.SetEnd(
                        table.FromEmployeeManageId.Value,
                        createEmployeeManageDlDto.StartOn.AddDays(-1),
                        createEmployeeManageDlDto.DocId,
                        TableIdConst.DOC_APPOINT_EMPLOYEE);

                    if (entityToDetach != null)
                        _unitOfWork.Context.Entry(entityToDetach).State = EntityState.Modified;

                    CombineStatuses(_employeeManageRepository);

                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }

                    _unitOfWork.Save();*/

                    if (exists)
                    {
                        var mc = new AutoMapper.MapperConfiguration(cfg =>
                        {
                            cfg.CreateMap<CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>();
                        });
                        var updateDlDto = mc.CreateMapper().Map<UpdateEmployeeManageDlDto>(createEmployeeManageDlDto);
                        _employeeManageRepository.Update(updateDlDto);
                    }
                    else
                    {
                        _employeeManageRepository.Create(createEmployeeManageDlDto);
                    }
                    CombineStatuses(_employeeManageRepository);
                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }
                    _unitOfWork.Save();
                }
                else if (createEmployeeManageDlDto.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL)
                {
                    #region chek employee is he have not signed documnet
                    int unsignedDocumentCount = GetUnsignedDocumentCount(table.EmployeeManageId.Value);

                    if (unsignedDocumentCount > 0)
                    {
                        UpdateDocumentsToChosenEmployee(table.FromEmployeeManageId.Value, table.ChoosenEmployeeManageId.Value);
                    }
                    #endregion

                    #region If hase ChoosenEmployee 

                    var choosenEmpManage = _unitOfWork.Context.Set<EmployeeManage>().AsNoTracking().FirstOrDefault(a => a.Id == table.ChoosenEmployeeManageId);

                    bool existsChoosen = _unitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                                         .Any(a => a.Id == table.EmployeeManageId);
                    if (choosenEmpManage != null)
                    {
                        var createChossenEmployeeManageDlDto = new CreateEmployeeManageDlDto
                        {
                            Id = table.EmployeeManageId.Value,
                            DocId = entity.Id,
                            DocTableId = TableIdConst.DOC_APPOINT_EMPLOYEE,
                            EmpAppointOrderTypeId = table.EmpAppointOrderTypeId,
                            StartOn = table.StartOn,
                            DepartmentId = table.DepartmentId.HasValue ? table.DepartmentId.Value : 0,
                            PositionId = table.PositionId.HasValue ? table.PositionId.Value : 0,
                            EmploymentTypeId = table.EmploymentTypeId.HasValue ? table.EmploymentTypeId.Value : 0,
                            EmploymentRate = table.EmployeeRate.HasValue ? table.EmployeeRate.Value : 0,
                            WorkScheduleId = table.WorkScheduleId.HasValue ? table.WorkScheduleId.Value : 0,
                            EmployeeId = choosenEmpManage.EmployeeId,
                            EndByDocumentOn = table.EndOn,
                            OrganizationId = entity.OrganizationId.Value,
                        };

                        if (existsChoosen)
                        {
                            var mc = new AutoMapper.MapperConfiguration(cfg =>
                            {
                                cfg.CreateMap<CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>();
                            });
                            var updateDlDto = mc.CreateMapper().Map<UpdateEmployeeManageDlDto>(createChossenEmployeeManageDlDto);
                            _employeeManageRepository.Update(updateDlDto);
                        }
                        else
                        {
                            _employeeManageRepository.Create(createChossenEmployeeManageDlDto);
                        }
                    }
                    #endregion

                    //_employeeManageRepository.SetEnd(
                    //    table.FromEmployeeManageId.GetValueOrDefault(),
                    //    createEmployeeManageDlDto.StartOn,
                    //    createEmployeeManageDlDto.DocId,
                    //    TableIdConst.DOC_APPOINT_EMPLOYEE);

                    //CombineStatuses(_employeeManageRepository);
                    using (var command = _unitOfWork.Context.Database.GetDbConnection().CreateCommand())
                    {

                        command.CommandText = @$"UPDATE hrm.sys_employee_manage SET 
                                                        end_table_id = @end_table_id,
                                                        end_doc_id = @end_doc_id,
                                                        end_on = @end_on WHERE id = @id";

                        command.Parameters.Add(new Npgsql.NpgsqlParameter("end_table_id", TableIdConst.DOC_APPOINT_EMPLOYEE));
                        command.Parameters.Add(new Npgsql.NpgsqlParameter("end_doc_id", createEmployeeManageDlDto.DocId));
                        command.Parameters.Add(new Npgsql.NpgsqlParameter("end_on", createEmployeeManageDlDto.StartOn.AddDays(-1)));
                        command.Parameters.Add(new Npgsql.NpgsqlParameter("id", table.FromEmployeeManageId.Value));

                        command.ExecuteNonQuery();
                    }

                    #region Delete From Turniket If Employee DISMISSAL
                    if (IsValid)
                    {
                        var entryDismissal = _unitOfWork.Context.Set<Employee>().Include(a => a.Organization)
                            .Include(a => a.Person)
                            .FirstOrDefault(a => a.Id == table.EmployeeId);

                        if (entryDismissal.Organization.OrganizationGroupId == OrganizationGroupIdConst.SSP
                            || entryDismissal.Organization.OrganizationGroupId == OrganizationGroupIdConst.ORGANIZATIONS_UNDER_SSP)
                        {
                            var deleteTurnstilePersonResult = await _wbacClientService.DeletePersonAsync(new WbacTurnstilePersonDto
                            {
                                PersonId = entryDismissal.Id,
                                TableId = TableIdConst.HL_EMPLOYEE,
                                OrganizationId = entryDismissal.OrganizationId,
                                FullName = entryDismissal.Person.FullName,
                                ImageId = entryDismissal.Person.PictureId != null ? entryDismissal.Person.PictureId.ToString() : null
                            });

                            if (!deleteTurnstilePersonResult.IsSuccess || deleteTurnstilePersonResult.Response)
                                CombineStatuses(deleteTurnstilePersonResult.GetStatusGeneric());
                        }
                    }
                    #endregion

                    var data = table.EmployeeId;
                    var data2 = _authService.User.OrganizationId;

                    var mmmm = table.FromEmployeeManageId.GetValueOrDefault();
                    var entityTable = _unitOfWork.Context.Set<AppointEmployeeTable>()
                        .FirstOrDefault(a => a.Owner.OrganizationId == _authService.User.OrganizationId &&
                                           a.EmployeeId == table.EmployeeId &&
                                 a.EmployeeManageId == mmmm);
                    if (entityTable != null)
                    {
                        entityTable.EndOn = createEmployeeManageDlDto.StartOn;

                    }

                    if (HasErrors)
                    {
                        if (canCommit)
                            transaction.Rollback();
                        return null;
                    }
                    _unitOfWork.Save();

                }
            }
            #endregion

            var updateResult = _repository.UpdateStatus(dto, ent =>
            {
                if (!StatusIdConst.CanApplyAppointEmployee(ent.StatusId, dto.StatusId))
                    _repository.AddError("Нет доступа");
            });

            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            _unitOfWork.Save();

            #region Create User
            foreach (AppointEmployeeTableDto table in entity.Tables)
            {
                if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
                {
                    var user = _repository.Context.Set<User>().AsNoTracking()
                            .FirstOrDefault(a => a.EmployeeManageId == table.EmployeeManageId.Value);

                    if (user == null)
                    {
                        var employee = _unitOfWork.Context.Set<Employee>().Include(a => a.Person).FirstOrDefault(a => a.Id == table.EmployeeId);

                        var defaultRoles = _unitOfWork.RoleRepository.AllAsQueryable
                                         .IsActive()
                                         .Where(a => a.Id == RoleIdConst.DOCUMENT_OBOROT
                                                  || a.Id == RoleIdConst.SIGNER_OF_HRM_DOCUMENTS)
                                         .Select(a => a.Id)
                                         .ToList();

                        User useforChek = _unitOfWork.Context.Set<User>().AsNoTracking().FirstOrDefault(a => a.PhoneNumber == Regex.Replace(employee.PhoneNumber, @"\D", "") && a.StateId == StateIdConst.ACTIVE);

                        if (useforChek != null)
                        {

                            CreateErrorResponseModel.ErrorName = useforChek.UserName;
                            CreateErrorResponseModel.ErrorId = useforChek.Id;

                            // AddError($"Bu {useforChek.PhoneNumber} nomer bilan {useforChek.UserName} foydalanuvchi ochilgan / Hodim nomerini o'zgartirin");
                        }

                        if (HasErrors)
                        {
                            if (canCommit)
                                transaction.Rollback();
                            return null;
                        }

                        if (!_unitOfWork.UserRepository.AllAsQueryable.Any(a => a.Person.PassportSeria == employee.Person.PassportSeria && a.Person.PassportNumber == employee.Person.PassportNumber))
                        {
                            var userName = $"{employee.Person.PassportSeria}-{employee.Person.PassportNumber}";
                            bool hasUserName = true;

                            int userNameIndex = 1;
                            while (hasUserName)
                            {
                                if (_unitOfWork.Context.Set<User>().Any(a => a.UserName == userName && a.StateId == StateIdConst.ACTIVE))
                                {
                                    userName = userName + userNameIndex;
                                    userNameIndex++;
                                }
                                else
                                    hasUserName = false;
                            }

                            var userForCreate = _userService.Create(new CreateUserDto
                            {
                                LanguageId = LanguageIdConst.RU,
                                OrganizationId = entity.OrganizationId.Value,
                                PersonId = employee.PersonId,
                                PhoneNumber = Regex.Replace(employee.PhoneNumber, @"\D", ""),
                                EmployeeManageId = table.EmployeeManageId,
                                Person = new CreatePersonDlDto
                                {
                                    Pinfl = employee.Person.Pinfl
                                },
                                UserName = userName,
                                Password = userName,
                                Roles = defaultRoles
                            });
                        }
                        CombineStatuses(_userService);
                        if (HasErrors)
                            return null;

                        _unitOfWork.Context.SaveChanges();
                    }
                }
            }
            #endregion

            #region Edoc ApoinmentOrder Change

            foreach (var item in updateResult.Tables)
            {
                var user = _repository.Context.Set<User>().AsNoTracking()
                            .FirstOrDefault(x => x.EmployeeManageId.HasValue
                                && (
                                    (item.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL && item.FromEmployeeManageId.HasValue && x.EmployeeManageId.Value == item.FromEmployeeManageId.Value)
                                    || (item.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL && item.EmployeeManageId.HasValue && x.EmployeeManageId.Value == item.FromEmployeeManageId.Value && x.StateId == StateIdConst.ACTIVE)));

                if (user != null)
                {

                    if (item.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
                    {
                        if (user != null)
                        {
                            user.StateId = StateIdConst.ACTIVE;
                            _unitOfWork.Context.Update(user);
                            var appointmentOrderDto = _userService.AppointmentOrderMap(updateResult, item.EmployeeManageId.Value);
                            _userService.UpdateUserForEdocSchema(user, appointmentOrderDto);
                            CombineStatuses(_userService);
                            if (HasErrors)
                            {
                                if (canCommit)
                                    transaction.Rollback();
                                return null;
                            }
                        }
                    }
                    else if (item.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                    {
                        if (user != null)
                        {
                            user.EmployeeManageId = item.EmployeeManageId;
                            user.StateId = StateIdConst.ACTIVE;
                            _unitOfWork.Context.Update(user);
                            var appointmentOrderDto = _userService.AppointmentOrderMap(updateResult, item.EmployeeManageId.Value);
                            _userService.UpdateUserForEdocSchema(user, appointmentOrderDto);
                            CombineStatuses(_userService);
                            if (HasErrors)
                            {
                                if (canCommit)
                                    transaction.Rollback();
                                return null;
                            }
                        }

                    }
                    else if (item.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL)
                    {
                        user.StateId = StateIdConst.PASSIVE;
                        _unitOfWork.Context.Update(user);
                        var appointmentOrderDto = _userService.AppointmentOrderMap(updateResult, item.FromEmployeeManageId.Value);
                        _userService.UpdateUserForEdocSchema(user, appointmentOrderDto);
                        CombineStatuses(_userService);
                        if (HasErrors)
                        {
                            if (canCommit)
                                transaction.Rollback();
                            return null;
                        }
                    }
                }
            }

            CombineStatuses(_userService);
            if (HasErrors)
            {
                if (canCommit)
                    transaction.Rollback();
                return null;
            }

            _unitOfWork.Context.SaveChanges();
            #endregion

            var result =
                CreateDocumentChangeLog(dto.Id,
                                        dto.StatusId, dto.Message);

            if (result != null)
                if (canCommit || IsValid)
                    transaction.Commit();


            return new CreateUserResponseModel
            {
                Id = CreateErrorResponseModel.ErrorId,
                UserName = CreateErrorResponseModel.ErrorName,
            };
        }
        catch (Exception ex)
        {
            AddError(ex.Message + ex.InnerException);
        }
        return null;
    }

    public HaveId<long> Cancel(UpdateStatusAppointEmployeeDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try 
            {
                Repository.AllAsQueryable.Lock(dto.Id);

                var statusDto = new UpdateStatusAppointEmployeeDlDto { Id = dto.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
                var entity = _repository.UpdateStatus(statusDto, null, applyFilter: false);
                var entity2 = _repository.AllAsQueryable/*.Include(a => a.Signer)*/.Include(a => a.Tables).FirstOrDefault(a => a.Id == dto.Id);
                CombineStatuses(_repository);
                if (HasErrors)
                    return null;
                var entityDto = _repository.ById<AppointEmployeeDto>(dto.Id, applyFilter: false);

                foreach (var table in entityDto.Tables)
                {
                    if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
                    {
                        var hasAcceptedDocument = _repository.AllAsQueryable
                          .Any(a => a.StatusId == StatusIdConst.ACCEPTED && a.Id != dto.Id && a.Tables.Any(b => b.EmployeeId == table.EmployeeId && b.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER && b.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL));

                        if (hasAcceptedDocument)
                        {
                            AddError($"{table.EmployeeFull} hodimga nisbatan 'boshqa lavozimga otkazish yoki ishdan bo'shatish' hujjat imzolangan shu sabab bu hujjatni bekor qib bo'lmaydi");
                            return null;
                        }

                    }

                    if (table.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL)
                    {
                        var user = _repository.Context.Set<User>()
                            .FirstOrDefault(x => x.EmployeeManageId.HasValue
                                && (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE
                                    ? x.EmployeeManageId.Value == table.EmployeeManageId
                                    : x.EmployeeManageId.Value == table.FromEmployeeManageId
                                )
                            );

                        if (user != null)
                        {
                            user.StateId = StateIdConst.PASSIVE;
                            _unitOfWork.Context.Update(user);
                            _userService.UpdateUserForEdocSchema(user);
                            CombineStatuses(_userService);
                            if (HasErrors)
                                return null;
                            //AddError($"Ҳужжатни бекор қилиб бўлмайди ушбу фойдаланувчига Манаге Ид лар бириктирилган. {user.UserName + " - " + user.EmployeeManageId.Value.ToString()}", nameof(table.EmployeeManageId));
                            //return null;
                        }
                    }
                    else
                    {
                        var user = _repository.Context.Set<User>()
                            .FirstOrDefault(x => x.EmployeeManageId.HasValue
                                && x.EmployeeManageId.Value == table.FromEmployeeManageId
                            );
                        if (user != null)
                        {
                            user.StateId = StateIdConst.ACTIVE;
                            _unitOfWork.Context.Update(user);
                            var appointmentOrderDto = _userService.AppointmentOrderMap(entity2, table.FromEmployeeManageId.Value);
                            _userService.UpdateUserForEdocSchema(user, appointmentOrderDto);
                            CombineStatuses(_userService);
                            if (HasErrors)
                                return null;
                        }

                    }

                    if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL)
                    {
                        _employeeManageRepository.SetIsDeleted(table.FromEmployeeManageId.Value, false);
                        #region ChoosenEmployee
                        if (table.ChoosenEmployeeManageId.HasValue)
                        {
                            var choosenemployee = _employeeManageRepository.SetIsDeleted(table.ChoosenEmployeeManageId.Value, false);
                        }
                        #endregion
                    }
                    else if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                    {
                        _employeeManageRepository.SetIsDeleted(table.FromEmployeeManageId.Value, false);
                        var employeeManage = _employeeManageRepository.SetIsDeleted(table.EmployeeManageId.Value, true);
                        if (employeeManage.EndOn.HasValue)
                            AddError($"Бу ишга қабул қилиш ҳужжатини бекор қилиб бўлмайди. {employeeManage.EndOn.Value} санада ходимга янги ҳужжат ({employeeManage.EndDocId}) яратилган", nameof(employeeManage.EndDocId));
                    }
                    else
                    {
                        var employeeManage = _employeeManageRepository.SetIsDeleted(table.EmployeeManageId.Value, true);
                        if (employeeManage.EndOn.HasValue)
                            AddError($"Бу ишга қабул қилиш ҳужжатини бекор қилиб бўлмайди. {employeeManage.EndOn.Value} санада ходимга янги ҳужжат ({employeeManage.EndDocId}) яратилган", nameof(employeeManage.EndDocId));
                    }
                }

                if (HasErrors)
                    return null;

                var result = CreateDocumentChangeLog(statusDto.Id,
                                       statusDto.StatusId, statusDto.Message);
                if (IsValid)
                {
                    if (dto.Message != null)
                        entity2.Message = dto.Message;

                    UnitOfWork.Save();
                    transaction.Commit();
                    return HaveId.Create(entity.Id);
                }

            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}: {ex.Source}");
                transaction.Rollback();
            }
            return null;
        }
    }

    public async Task Sign(SignStatusAppointEmployeeDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
			try
            {
				var entity = _repository.ById<AppointEmployeeDto>(dto.Id);

                #region Validation
                
                if (entity.Tables.Any(a => a.PositionId == null && a.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL))
                    AddError($"Ҳодимга лавозим бириктирилмаган !");
                
                var noStaffingPosition = _unitOfWork.Context.Set<StaffingPosition>()
                    .Where(a => a.Owner.OrganizationId == _authService.User.OrganizationId && a.Owner.StatusId != StatusIdConst.ARCHIVED).Select(a => a.PositionId).ToArray();

                var hireTable = entity.Tables.Where(x => x.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL);
                if (hireTable.Any())
                {
                    Dictionary<string, decimal> staffings = _unitOfWork.Context.Set<Staffing>()
                        .Include(a => a.Positions)
                        .Where(a => a.OrganizationId == _authService.User.OrganizationId
                            && a.StatusId == StatusIdConst.ACCEPTED
                            && a.DocOn <= entity.DocOn
                        )
                        .SelectMany(a => a.Positions)
                        .Select(a => new
                        {
                            a.PositionClassificationId,
                            a.DepartmentId,
                            a.Quantity
                        })
                        .AsEnumerable()
                        .GroupBy(a => new { a.PositionClassificationId, a.DepartmentId })
                        .ToDictionary(
                            a => $"{a.Key.PositionClassificationId}-{a.Key.DepartmentId}",
                            a => a.Sum(b => b.Quantity)
                        );

                    var temporaryTable = entity.Tables.Where(x => x.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                                                      .Select(x => x.FromEmployeeManageId);

                    Dictionary<string, decimal> employeeManage = _unitOfWork.EmployeeManageRepository.AllAsQueryable
                        .AsNoTracking()
                        .Where(a => a.EndOn == null)
                        .Include(a => a.Position)
                        .AsEnumerable()
                        .GroupBy(a => new
                        {
                            a.Position.PositionClassificationId,
                            a.DepartmentId
                        })
                        .ToDictionary(
                            a => $"{a.Key.PositionClassificationId}-{a.Key.DepartmentId}",
                            a => a.Sum(b => b.EmploymentRate.Value)
                        );

                    foreach (var staffing in staffings)
                        if (employeeManage.ContainsKey(staffing.Key))
                            staffings[staffing.Key] -= employeeManage[staffing.Key];

                    if (HasErrors)
                        return;

                    Dictionary<string, decimal> positions = new Dictionary<string, decimal>();

                    Dictionary<string, string> positionName = new Dictionary<string, string>();

                    foreach (var table in hireTable.GroupBy(a => new
                    {
                        a.PositionId,
                        a.DepartmentId
                    }))
                    {
                        var keyPos = table.Key.PositionId.HasValue ? table.Key.PositionId.Value.ToString() : "";
                        string key = $"{keyPos}-{table.Key.DepartmentId}";

                        if (table.Key.PositionId.HasValue &&
                            !positions.ContainsKey(key))
                        {
                            positions.Add(key, table.Sum(a => a.EmployeeRate.Value));
                            positionName.Add(key, table.FirstOrDefault(x => x.PositionId == table.Key.PositionId.Value).Position);
                        }
                    }

                    foreach (var table in positions)
                    {
                        string[] idPosition = table.Key.Split('-');
                        if (!noStaffingPosition.Contains(Int32.Parse(idPosition[0])))
                            if (!staffings.ContainsKey(table.Key))
                                AddError($"{positionName[table.Key]}  \nШтатда бундай лавозим классификацияси кўрсатилмаган.");
                            else if (staffings[table.Key] < table.Value)
                                AddError($"Штатда бундай лавозим классификацияси етарли эмас. Штатда ({staffings[table.Key]}-{positionName[table.Key]})");
                    }

                    if (HasErrors)
                        return;

                    // Ishdan bo'shatilayotgan yoki lavozimini o'zgartirayotgan hodimlar IsDeleted bo'lishi kerak emas!
                    var fromEmployeeManageIds = entity.Tables.Where(b => b.FromEmployeeManageId.HasValue).Select(b => b.FromEmployeeManageId).ToList();
                    var deletedEmployees = UnitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                        .Where(a => fromEmployeeManageIds.Contains(a.Id) && a.IsDeleted)
                        .Select(a => a.Employee.Person.FullName);
                    if (deletedEmployees.Any())
                        AddError($"Қуйидаги ходимларни ишга қабул қилиш хужжати бекор қилинган: {string.Join(", ", deletedEmployees)}");

                    // ishga qabul qilingan sanasi ishdan ketgan sanasidan kichkina bo'lishi kerak!
                    var employees = UnitOfWork.Context.Set<EmployeeManage>().AsNoTracking()
                        .Where(a => fromEmployeeManageIds.Contains(a.Id))
                        .Select(a => new { a.Employee.Person.FullName, a.StartOn });
                    if (employees.Any(a => a.StartOn >= entity.DocOn))
                        AddError($"Ишдан кетиш санаси ишга қабул қилинган санадан катта бўлиши керак.");

                    if (HasErrors)
                        return;

                    foreach (var table in entity.Tables)
                    {
                        if (table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE ||
                            table.EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
                        {
                            CheckEmploymentRate(table);

                            if (HasErrors)
                                return;
                        }
                    }
                }

                #endregion

                if (!_authService.HasPermission(ModuleCode.AppointEmployeeWithoutSigner) && entity.Signer.Count() == 0)
                {
                    _repository.AddError("Sizda Imzolovchilarsiz hujjatni qabul qilish vakolati yo'q");
                    CombineStatuses(_repository);

                    if (HasErrors)
                    {
                        return;
                    }
                }

                if (entity.Signer.Count() > 0)
                {
                    var allSigners = _unitOfWork.Context.AppointEmployeeSigner
                        .Where(a => a.OwnerId == dto.Id)
                        .OrderBy(a => a.SignOrder)
                        .ToList();

                    var currentSigner = allSigners.FirstOrDefault(a => a.EmployeeManageId == _authService.User.EmployeeManageId);
                    if (currentSigner == null)
                    {
                        AddError($"Sizda imzolash huquqi yo'q...");
                        CombineStatuses(_repository);
                        transaction.Rollback();
                        return;
                    }

                    var previousSigner = allSigners.Where(a => a.SignOrder < currentSigner.SignOrder).OrderByDescending(a => a.SignOrder).FirstOrDefault();
                    if (previousSigner != null && !previousSigner.SignedAt.HasValue)
                    {
                        AddError($"Avval {previousSigner.EmployeeManageId} (SignOrder = {previousSigner.SignOrder}) imzolashi kerak!");
                        CombineStatuses(_repository);
                        transaction.Rollback();
                        return;
                    }

					var eImzoTimstampDto = new EImzoTimeStampDto
                    {
                        SignData = dto.SignedData,
                        Inn = null,
                        Pinfl = _authService.User.Pinfl
                    };

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

					currentSigner.SignFile = SaveFile(dto.Id, timeStamp.Pkcs7b64, "sign.txt");
                    currentSigner.DataFile = SaveFile(dto.Id, JsonConvert.SerializeObject(dto), "data.txt");
                    currentSigner.SignedAt = DateTime.Now;

                    _unitOfWork.Context.Entry(currentSigner).State = EntityState.Modified;
                    _unitOfWork.Save();

                    var ent = _repository.UpdateStatus(dto, ent =>
                    {
                        if (!StatusIdConst.CanApplyAppointEmployee(ent.StatusId, dto.StatusId))
                            _repository.AddError("Нет доступа");
                    });
                   
                    CombineStatuses(_repository);
                    if (HasErrors)
                    {
						transaction.Rollback();
						return;
					}
                        
                    _unitOfWork.Save();
                    if (IsValid)
                    {
                        bool isLastSigner = allSigners.All(a => a.SignedAt.HasValue);

                        var res = CreateDocumentChangeLog(entity.Id, StatusIdConst.SIGNING, Message);

                        if (isLastSigner)
                        {
                            await AcceptAsync(new UpdateStatusAppointEmployeeDto
                            {
                                Id = dto.Id,
                                StatusId = StatusIdConst.ACCEPTED
                            });
                        }

                        transaction.Commit();
                    }
                }
                else
                {
                    await AcceptAsync(new UpdateStatusAppointEmployeeDto
                    {
                        Id = dto.Id,
                        StatusId = StatusIdConst.ACCEPTED
                    });
                }
            }

            catch (Exception ex)
            {
                AddError($"{ex.StackTrace} {ex.InnerException} {ex.Message}");
                transaction.Rollback();
            }
        }
    }

    public HaveId<long> CreateDocumentChangeLog(long id, int statusId, string message = null)
    {
        var moveDto = _repository.ById<AppointEmployeeDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.DOC_APPOINT_EMPLOYEE,
            organizationId: null,
            statusId: statusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    private void Validation<TDto>(AppointEmployeeDlDto<TDto> dto, AppointEmployee entity)
       where TDto : AppointEmployeeDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        var currentYear = DateTime.Now.Year;

        if (query.ByDocNumber(dto.DocNumber, currentYear).Any())
            _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

        foreach (var tableItem in dto.Tables.Where(a => a.EmpAppointOrderTypeId != EmpAppointOrderTypeIdConst.DISMISSAL))
        {
            //var staffingPositions = _unitOfWork.Context.Set<StaffingPosition>()
            //.Include(a => a.Owner)
            //.OrderByDescending(a => a.Owner.DocOn)
            //.FirstOrDefault(a => a.Owner.StatusId == StatusIdConst.RECEIVED &&
            //                     a.Owner.OrganizationId == _authService.User.OrganizationId &&
            //                     a.PositionId == tableItem.PositionId &&
            //                     a.DepartmentId == tableItem.DepartmentId);

            var staffingPositionsQuery = _unitOfWork.Context.Set<StaffingPosition>()
                                                           .Include(a => a.Owner)
                                                           .Where(a => a.Owner.StatusId == StatusIdConst.RECEIVED)
                                                           .OrderByDescending(a => a.Owner.DocOn);

            StaffingPosition staffingPosition;

            if (_authService.HasPermission(ModuleCode.AllAppointEmployeeCreate))
            {
                staffingPosition = staffingPositionsQuery
                    .FirstOrDefault(a => a.Owner.OrganizationId == dto.OrganizationId &&
                                         a.PositionId == tableItem.PositionId &&
                                         a.DepartmentId == tableItem.DepartmentId);
            }
            else
            {
                staffingPosition = staffingPositionsQuery
                    .FirstOrDefault(a => a.Owner.OrganizationId == _authService.User.OrganizationId &&
                                         a.PositionId == tableItem.PositionId &&
                                         a.DepartmentId == tableItem.DepartmentId);
            }


            var anyAppointEmployees = _unitOfWork.Context.Set<EmployeeManage>()
                .Where(b => b.OrganizationId == _authService.User.OrganizationId && b.EndOn == null && b.IsDeleted == false && b.PositionId == staffingPosition.PositionId && b.DepartmentId == staffingPosition.DepartmentId);

            if ((entity == null ||
                (entity != null && !anyAppointEmployees.Any(a => a.Id == entity.Id)))
                && staffingPosition.Quantity <= anyAppointEmployees.Sum(a => a.EmploymentRate))
                AddError("Ушбу лавозимда ставка етарли эмас // Недостаточно ставок для данной должности");
        }
        foreach (var tableItem in dto.Tables)
        {
            if (tableItem.Acting && (tableItem.EmployeeRate == null || tableItem.EmployeeRate == 0))
            {
                AddError("Hodim uchun stavkani kiriting");
            }
            else if (tableItem.Interm && tableItem.EmployeeRate == null)
            {
                AddError("Vazifasini bajaruvchi hodim uchun stavkani 0 kiritng");
            }
        }
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

    private Guid SaveFile(long docId, string data, string fileName)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(data);
        writer.Flush();
        ms.Position = 0;
        var fileInfo = _storageService.Save($"{nameof(TableIdConst.DOC_APPOINT_EMPLOYEE)}_SIGN_DATA", docId.ToString(), new StorageFile(Guid.NewGuid(), fileName, ms));

        return fileInfo.FirstOrDefault().FileId;
    }

    public byte[] GenerateWord()
    {
        //var plh = new Placeholders();
        var dto = new AppointEmployeeDto();
        if (string.IsNullOrEmpty(dto.Details))
            dto.Details = " ";
		if (string.IsNullOrEmpty(dto.ConclusionForPrint))
			dto.ConclusionForPrint = " ";
		var file = WordFactory.GenerateWordTemplate(dto);
        return file;
    }

    public async ValueTask<byte[]> Print(long id)
    {
        var dto = Repository.ById<AppointEmployeeDto>(id);
        dto.QrCode = new()
        {
            Text = "Test Qr Code. :)  Test Qr Code. :)  Test Qr Code. :)  Test Qr Code. :)",
            Dpi = 180,
            Width = 256,
            Height = 256,
        };
        return await _print.Print<AppointEmployeeDto, long>(dto);
    }

    public async Task<byte[]> DownloadPdf(Guid id2, string lange)
    {
        var language = lange ?? "uz-latn";
        //lang = "ru";
        //lang = "ru";
        var dto = Repository.ReadAsNoTracked<AppointEmployeeDto>(applyFilter: false)
                            .FirstOrDefault(x => x.Id2 == id2);

        #region Capitalize
        foreach (var item in dto.Employees)
        {
            item.FullName = item.FullName.CapitalizeEachWord(" ");
        }
        foreach (var item in dto.Tables)
        {
            item.EmployeeFull = item.EmployeeFull.CapitalizeEachWord(" ");
            item.Organization = item.Organization.CapitalizeEachWord(" ");
        }

        dto.Organization = dto.Organization.CapitalizeEachWord(" ");
        #endregion

        MemoryStream wordFile = null;
        if (dto.Tables.FirstOrDefault().EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE)
        {
            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            language,
            StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_HIRE));

            if (dto == null)
            {
                AddError("Hujjat topilmadi.");
                return null;
            }

            for (int i = 0; i < dto.Tables.Count; i++)
            {
                dto.Tables[i].index = i + 1;
            }
            dto.Signer.OrderByDescending(x => x.SignOrder);

            for (int i = 0; i < dto.Signer.Count; i++)
            {

                if (dto.Signer[i].IsDirector)
                    dto.Signer[i].Position = dto.Signer[i].Position + " ";
                else if (dto.Signer[i].IsHr)
                    dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
                else
                    dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSign = null;
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSignValue = " ";
            }
        }
        else if (dto.Tables.FirstOrDefault().EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.HIRE && dto.Tables.FirstOrDefault().IsProbation == true)
        {
            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            language,
            StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_HIRE_IS_PROBARATION));

            if (dto == null)
            {
                AddError("Hujjat topilmadi.");
                return null;
            }

            for (int i = 0; i < dto.Tables.Count; i++)
            {
                dto.Tables[i].index = i + 1;
            }
            dto.Signer.OrderByDescending(x => x.SignOrder);
            for (int i = 0; i < dto.Signer.Count; i++)
            {

                if (dto.Signer[i].IsDirector)
                    dto.Signer[i].Position = dto.Signer[i].Position + " ";
                else if (dto.Signer[i].IsHr)
                    dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
                else
                    dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSign = null;
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSignValue = " ";
            }
        }
        else if (dto.Tables.FirstOrDefault().EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.TRANSFER)
        {
            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            language,
            StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_TRANSFER));

            if (dto == null)
            {
                AddError("Hujjat topilmadi.");
                return null;
            }

            for (int i = 0; i < dto.Tables.Count; i++)
            {
                dto.Tables[i].index = i + 1;
            }
            dto.Signer.OrderByDescending(x => x.SignOrder);
            for (int i = 0; i < dto.Signer.Count; i++)
            {

                if (dto.Signer[i].IsDirector)
                    dto.Signer[i].Position = dto.Signer[i].Position + " ";
                else if (dto.Signer[i].IsHr)
                    dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
                else
                    dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";
                if (!dto.Signer[i].SignedAt.HasValue)
                       dto.Signer[i].QrSign = null;
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSignValue = " ";

            }
        }
        else if (dto.Tables.FirstOrDefault().EmpAppointOrderTypeId == EmpAppointOrderTypeIdConst.DISMISSAL)
        {
            wordFile = _storageService.GetStaticFile(
               StaticFileConst.WordTemplate.GetFileName(
            language,
            StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_DISMISSAL));

            if (dto == null)
            {
                AddError("Hujjat topilmadi.");
                return null;
            }

            for (int i = 0; i < dto.Tables.Count; i++)
            {
                dto.Tables[i].index = i + 1;
            }
            dto.Signer.OrderByDescending(x => x.SignOrder);
            for (int i = 0; i < dto.Signer.Count; i++)
            {
                if (dto.Signer[i].IsDirector)
                    dto.Signer[i].Position = dto.Signer[i].Position + " ";
                else if (dto.Signer[i].IsHr)
                    dto.Signer[i].Position = "Kiritildi: <br/>" + dto.Signer[i].Position + " ";
                else
                    dto.Signer[i].Position = "Kelishildi: <br/>" + dto.Signer[i].Position + " ";
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSign = null;
                if (!dto.Signer[i].SignedAt.HasValue)
                    dto.Signer[i].QrSignValue = " ";

            }
        }

        if (string.IsNullOrEmpty(dto.Details)) 
            dto.Details = " ";
        if (string.IsNullOrEmpty(dto.ConclusionForPrint)) 
            dto.ConclusionForPrint = " ";
        var plh = WordFactory.MakePlaceholders(dto);
        var handler = new DocXHandler(wordFile, plh);
        handler.ReplaceLists();
        handler.ReplaceTexts();
        handler.ReplaceImages();
        var res = await _pdfConverter.DocxToPdfAsync(wordFile, new());
        CombineStatuses(_pdfConverter);
        return res;
        //return null;
    }

    #region Is employee is Signer in other documnet 
    private int GetUnsignedDocumentCount(long empManageId)
    {
        int totalDocumentTypes = 6; // Update this with the actual number of document types you're checking

        var signedDocumentCount = 0;

        signedDocumentCount += _unitOfWork.Context.Set<AppointEmployeeSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);
        signedDocumentCount += _unitOfWork.Context.Set<ChastisementSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);
        signedDocumentCount += _unitOfWork.Context.Set<EmployeeLeaveOrderSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);
        signedDocumentCount += _unitOfWork.Context.Set<EmployeeSendTrainSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);
        signedDocumentCount += _unitOfWork.Context.Set<OrderToSendBusinessTripSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);
        signedDocumentCount += _unitOfWork.Context.Set<RecallLeaveSigner>().Count(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty);

        int unsignedDocumentCount = signedDocumentCount - totalDocumentTypes;
        return unsignedDocumentCount;
    }

    private List<DocumentForHrm> GetUnsignedDocuments(long empManageId)
    {
        var unsignedDocuments = new List<DocumentForHrm>();

        var unsignedAppointEmployeeSigners = _unitOfWork.Context.Set<AppointEmployeeSigner>()
            .Include(a => a.Owner)
            .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
            .Select(signer => new DocumentForHrm
            {
                DocId = signer.Owner.Id,
                Details = signer.Owner.Details,
                SignerTableId = signer.Id,
                EmployeeManageId = empManageId
            })
            .ToList();

        unsignedDocuments.AddRange(unsignedAppointEmployeeSigners);

        var unsignedChastisementSigners = _unitOfWork.Context.Set<ChastisementSigner>()
            .Include(a => a.Owner)
            .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
            .Select(signer => new DocumentForHrm
            {
                DocId = signer.Owner.Id,
                Details = signer.Owner.Details,
                SignerTableId = signer.Id,
                EmployeeManageId = empManageId
            })
            .ToList();

        unsignedDocuments.AddRange(unsignedChastisementSigners);

        var unsignedEmployeeLeaveOrderSigners = _unitOfWork.Context.Set<EmployeeLeaveOrderSigner>()
            .Include(a => a.Owner)
            .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
            .Select(signer => new DocumentForHrm
            {
                DocId = signer.Owner.Id,
                Details = signer.Owner.Details,
                SignerTableId = signer.Id,
                EmployeeManageId = empManageId
            })
            .ToList();

        unsignedDocuments.AddRange(unsignedEmployeeLeaveOrderSigners);

        var unsignedEmployeeSendTrainSigners = _unitOfWork.Context.Set<EmployeeSendTrainSigner>()
            .Include(a => a.Owner)
            .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
            .Select(signer => new DocumentForHrm
            {
                DocId = signer.Owner.Id,
                Details = signer.Owner.Details,
                SignerTableId = signer.Id,
                EmployeeManageId = empManageId
            })
            .ToList();

        unsignedDocuments.AddRange(unsignedEmployeeSendTrainSigners);

        var unsignedOrderToSendBusinessTripSigners = _unitOfWork.Context.Set<OrderToSendBusinessTripSigner>()
           .Include(a => a.Owner)
           .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
           .Select(signer => new DocumentForHrm
           {
               DocId = signer.Owner.Id,
               Details = signer.Owner.Details,
               SignerTableId = signer.Id,
               EmployeeManageId = empManageId
           })
           .ToList();

        unsignedDocuments.AddRange(unsignedOrderToSendBusinessTripSigners);

        var unsignedRecallLeaveSigners = _unitOfWork.Context.Set<RecallLeaveSigner>()
          .Include(a => a.Owner)
          .Where(signer => signer.EmployeeManageId == empManageId && signer.SignFile == Guid.Empty && signer.Owner.StatusId != StatusIdConst.ACCEPTED)
          .Select(signer => new DocumentForHrm
          {
              DocId = signer.Owner.Id,
              Details = signer.Owner.Details,
              SignerTableId = signer.Id,
              EmployeeManageId = empManageId
          })
          .ToList();

        unsignedDocuments.AddRange(unsignedRecallLeaveSigners);

        return unsignedDocuments;
    }

    private void UpdateDocumentsToChosenEmployee(long originalEmployeeId, long chosenEmployeeId)
    {

        var unsignedDocuments = GetUnsignedDocuments(originalEmployeeId);

        foreach (var document in unsignedDocuments)
        {
            document.EmployeeManageId = chosenEmployeeId;
        }
        _unitOfWork.Context.SaveChanges();
    }

    public byte[]? GetWordTemplate()
    {
        var lang = "uz-cyrl";

        var wordFile = _storageService.GetStaticFile(
            StaticFileConst.WordTemplate.GetFileName(lang, StaticFileConst.WordTemplate.APPOINT_EMPLOYEE_COMMON));

        CombineStatuses(_storageService);
        if (HasErrors)
            return null;

        return wordFile.ToArray();
    }

    public object UploadFiles(params StorageFile[] files)
    {
        if (files == null || files.Length <= 0)
        {
            AddError("Empty file");
            return null;
        }

        var result = _storageService.SaveTemp(DocumentStorageConst.DOC_APPOINT_EMPLOYEE_COMMON, files)
            .Select(a => new AppointEmployeeFileDto
            {
                Id = a.FileId,
                FileName = a.FileName,
                CreatedAt = DateTime.Now,
            });

        CombineStatuses(_storageService);
        return IsValid ? result : null;
    }

    public async Task SignUpdate(long id)
    {
        var entity = await _unitOfWork.Context.Set<AppointEmployee>()
            .Include(a => a.Signer)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (entity == null)
        {
            AddError("Hujjat topilmadi");
            return;
        }

        if (entity.Signer.Any())
        {
            var signers = entity.Signer.OrderBy(x => x.SignOrder).ToList();
            if (signers.Any(x => x.SignedAt == null))
            {
                AddError("Hujjat imzolash uchun barcha imzolovchilar imzolamagan");
                return;
            }

            var lastSigner = signers.LastOrDefault();
            if (lastSigner == null || _authService.User.EmployeeManageId != lastSigner.EmployeeManageId)
            {
                AddError("Siz hujjatni imzolash uchun ruxsatga ega emassiz");
                return;
            }

            var dto = new AcceptStatusAppointEmployeeDto
            {
                Id = id,
                StatusId = StatusIdConst.ACCEPTED
            };

            await AcceptAsync(new UpdateStatusAppointEmployeeDto
            {
                Id = dto.Id,
                StatusId = dto.StatusId,
            });
        }
    }

    #endregion
}