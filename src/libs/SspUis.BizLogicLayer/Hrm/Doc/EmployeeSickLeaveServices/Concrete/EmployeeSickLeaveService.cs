using Microsoft.EntityFrameworkCore;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveService
    : BaseEntityService<long, EmployeeSickLeave, EmployeeSickLeaveListDto, EmployeeSickLeaveDto, CreateEmployeeSickLeaveDlDto, UpdateEmployeeSickLeaveDlDto, IEmployeeSickLeaveRepository, EmployeeSickLeaveSortFilterOptions>
    , IEmployeeSickLeaveService
{
    IAuthService _authService;
    IUnitOfWork _unitOfWork;
    IDocumentChangeLogService _documentChangeLogService;
    private readonly IEmployeeSickLeaveRepository _repository;
    private readonly INumberService _numberService;
    public EmployeeSickLeaveService(
        IUnitOfWork unitOfWork,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService,
        IAuthService authService)
        : base(unitOfWork)
    {
        this._repository = unitOfWork.EmployeeSickLeaveRepository;
        this._authService = authService;
        this._unitOfWork = unitOfWork;
        this._documentChangeLogService = documentChangeLogService;
        this._numberService = numberService;
    }
    public PagedResult<EmployeeSickLeaveListDto> GetList(EmployeeLeaveOrderSortFilter dto)
    {
        //var result = _repository.CrudServices.ProjectFromEntityToDto<EmployeeSickLeave, EmployeeSickLeaveListDto>(
        //                    query => query.Where(a => ((dto.IsSelectList.HasValue &&
        //                                                dto.IsSelectList.Value == true &&
        //                                                dto.EmployeeId.HasValue) ? a.Tables.Any(t => t.EmployeeId == dto.EmployeeId) : true &&
        //                                                  a.StatusId == StatusIdConst.ACCEPTED) &&
        //                                              a.OrganizationId == _authService.User.OrganizationId &&
        //                                              a.StatusId != StatusIdConst.DELETED)
        //);

        //return result.SortFilter(dto)
        //                        .AsPagedResult(dto);
        var result = _repository.ReadAsNoTracked<EmployeeSickLeaveListDto>()
                                    .SortFilter(dto)
                                    .AsPagedResult(dto);
        return result;
    }
    //public PagedResult<UnpaidEmployeeSickLeaveListDto> GetUnPaidList(UnpaidEmployeeSickLeaveSortFilterPageOptions dto)
    //{
    //    if (!dto.StartDate.HasValue)
    //        dto.StartDate = DateTimeUtility.FirstDayOfYear(DateTime.Today);
    //    if (!dto.EndDate.HasValue)
    //        dto.EndDate = DateTimeUtility.LastDayOfYear(DateTime.Today);

    //    var calcLeavePayDocumentTableIds = _repository.Context.Set<CalcLeavePay>()
    //            .Where(a => a.OrganizationId == _authService.User.OrganizationId &&
    //                        a.StatusId != StatusIdConst.DELETED &&
    //                        a.DocumentTableId.HasValue &&
    //                        a.DocumentSysTableId == TableIdConst.DOC_EMPLOYEE_SICK_LEAVE)
    //            .Select(a => a.DocumentTableId);

    //    var result = _repository.CrudServices.ProjectFromEntityToDto<EmployeeSickLeaveTable, UnpaidEmployeeSickLeaveListDto>(a => a.Where(b => true))
    //                                      .Where(a => a.DocumentStatusId == StatusIdConst.ACCEPTED && !calcLeavePayDocumentTableIds.Contains(a.Id))
    //                                      .SortFilter(dto)
    //                                      .AsPagedResult(dto);
    //    return result;
    //}

    public SelectList<long> AsSelectList(int? employeeId = null)
    {
        return _repository.AllAsQueryable
            .Include(a => a.Tables)
            .Where(a => employeeId.HasValue ? a.Tables.Any(a => a.EmployeeId == employeeId.Value) : true && a.StatusId == StatusIdConst.ACCEPTED)
                        .AsSelectList();
    }
    public EmployeeSickLeaveDto Get()
    {
        return new EmployeeSickLeaveDto
        {
            DocOn = DateOnly.FromDateTime(DateTime.Now),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_EMPLOYEE_SICK_LEAVE, 1).Item2
        };
    }

    public EmployeeSickLeaveDto Get(long id)
    {
        var dto = _repository.ById<EmployeeSickLeaveDto>(id);
        var dto1 = _repository.ReadAsNoTracked<EmployeeSickLeaveDto>().FirstOrDefault(employeeSickLeave => employeeSickLeave.Id == id);
        CombineStatuses(_repository);
        if (IsValid)
        {
            dto.CanModify = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.MODIFIED) && _authService.HasPermission(ModuleCode.EmployeeSickLeaveEdit);
            dto.CanAccept = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeSickLeaveAccept);
            dto.CanCancel = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.NOT_ACCEPTED) && _authService.HasPermission(ModuleCode.EmployeeSickLeaveCancel);
            dto.CanDelete = StatusIdConst.CanApplyHrmDocStatus(dto.StatusId, StatusIdConst.DELETED) && _authService.HasPermission(ModuleCode.EmployeeSickLeaveDelete);
        }
        return dto;
    }

    public EmployeeSickLeaveTableDto GetByEmployeeId(int emplyeeId) //01.03.2024, 31,03,2024
    {

        var dto = _repository.ReadAsNoTracked<EmployeeSickLeaveDto>()
            .Where(a=>a.Tables
            .Any(t=>t.EmployeeId == emplyeeId) && a.StatusId == 2)
            .FirstOrDefault();

        var tableItem = dto?.Tables.FirstOrDefault(t => t.EmployeeId == emplyeeId);
        CombineStatuses(_repository);
        return tableItem;
    }

    //public async Task<IEnumerable<SickLeaveInfoDto>> GetSickLeaveInfoFromHrMf(string pinfl)
    //{
    //    try
    //    {
    //        var integrationData = await _hrMfSickInfoService.GetSickInfo(pinfl);
    //        CombineStatuses(_hrMfSickInfoService);

    //        if (HasErrors)
    //            return null;

    //        string format = "MMM d, yyyy h:mm:ss tt";

    //        if (integrationData.Items.Any())
    //        {
    //            var result = integrationData.Items.Select(item => new SickLeaveInfoDto
    //            {
    //                Id = item.Id,
    //                FullName = $"{item.Surname} {item.Firstname} {item.Patronymic}",
    //                OrganizationName = item.Applications.FirstOrDefault().OrganizationName,
    //                StartDate = item.Applications[0].Days.MinBy(day => day.StartDate).StartDate,
    //                EndDate = item.Applications[0].Days.MinBy(day => day.EndDate).EndDate,
    //                GivenDate = DateTime.ParseExact(s: item.CreatedAt,
    //                                                format: format,
    //                                                provider: CultureInfo.InvariantCulture),
    //                Seria = item.Applications[0].CertSer,
    //                Number = item.Applications[0].CertNum,
    //                Diagnos = item.Applications[0].Days
    //                                .Select(day => day.Diagnosis.NameRu)
    //                                    .FirstOrDefault(),
    //                IsClosed = item.Applications[0].Status.Code == "closed" ? true : false,
    //                IsChecked = true,
    //                Checked = IsCheckedSickList(item.Applications[0].Days),
    //                IsMaternityLeave = item.Applications[0].Days.FirstOrDefault().Reason.Id == 5
    //            });
    //            return result;
    //        }

    //        return null;

    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}

    //private bool IsCheckedSickList(List<DayDto> days)

    //{
    //    //5 => maternity-leave
    //    if (days.Any(day => day.Reason.Id == 5) &&
    //        days.Any(day => day.Reason.Id != 5))
    //        return true;

    //    return false;
    //}

    public override HaveId<long> Create(CreateEmployeeSickLeaveDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Create(dto, ent => Validation(dto, ent));
                UnitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, "EmployeeSickLeave");
                CombineStatuses(Repository);
                if(IsValid)
                    transaction.Commit();
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

    public override void Update(UpdateEmployeeSickLeaveDlDto dto)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.Update(dto, ent => {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, StatusIdConst.MODIFIED))
                        Repository.AddError("Нет доступа");
                    else
                        Validation(dto, ent);
                }
                );
                UnitOfWork.Save();
                //var res = CreateDocumentChangeLog(entity.Id, "EmployeeSickLeave");
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
    public void Accept(UpdateStatusEmployeeSickLeaveDto dTo)
    {
        var dto = new UpdateStatusEmployeeSickLeaveDlDto { Id = dTo.Id, StatusId = StatusIdConst.ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                Repository.AddError("Нет доступа");

            bool isDeleted = _unitOfWork.Context.Set<EmployeeManage>()
                .Where(a => ent.Tables.Select(a => a.EmployeeManageId).Contains(a.Id)).Any(a => a.IsDeleted);
            if (isDeleted)
                Repository.AddError("Сотрудник был удален");
        });
    }

    public void Cancel(UpdateStatusEmployeeSickLeaveDto dTo)
    {
        var dto = new UpdateStatusEmployeeSickLeaveDlDto { Id = dTo.Id, StatusId = StatusIdConst.NOT_ACCEPTED };
        UpdateStatus(dto, ent =>
        {
            if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, dto.StatusId))
                Repository.AddError("Нет доступа");

            //_hrmPackageContext.DocControlPackage.EmployeeSickLeaveCancelValidation(
            //    organizationId: _authService.Organization.idf,
            //    id: dto.Id,
            //    userId: (int)_authService.UserId
            //);
            //CombineStatuses(_hrmPackageContext.DocControlPackage);

        });
    }
    public async Task<byte[]> DownloadPdf(Guid id2)
    {
        return null;
    }
    public override void Delete(long id)
    {
        using(var transaction = UnitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.ById(id);
                var statusDto = new UpdateStatusEmployeeSickLeaveDlDto()
                {
                    Id = id,
                    StatusId = StatusIdConst.DELETED
                };
                UpdateStatus(statusDto, ent =>
                {
                    if (!StatusIdConst.CanApplyHrmDocStatus(ent.StatusId, statusDto.StatusId))
                        _repository.AddError("Нет доступа");
                });
                UnitOfWork.Save();

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
    private HaveId<long> UpdateStatus(UpdateStatusEmployeeSickLeaveDlDto dto, Action<EmployeeSickLeave> validation)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                var entity = Repository.UpdateStatus(dto, validation);
                CombineStatuses(Repository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();
                var res = CreateDocumentChangeLog(entity.Id, "EmployeeSickLeave");
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
    private HaveId<long> CreateDocumentChangeLog(long id, string message = null, string userIp = null, string userAgent = null)
    {
        var entityDto = Repository.ById<EmployeeSickLeaveDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: entityDto,
            tableId: TableIdConst.DOC_EMPLOYEE_SICK_LEAVE,
            organizationId: null,
            statusId: entityDto.StatusId,
            message: message,
            userIp: userIp,
            userAgent: userAgent);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }
    private void Validation<TDto>(EmployeeSickLeaveDlDto<TDto> dto, EmployeeSickLeave entity)
      where TDto : EmployeeSickLeaveDlDto<TDto>
    {
        if (dto.Checked.HasValue && dto.Checked.Value)
            Repository.AddError(
                errorMessage: "\"Тиббиёт ахборот тизими\"дан олинган электрон маълумотда ҳам касаллик ва шикастланганлик бўйича меҳнатга лаёқатсизлик, ҳам ҳомиладорлик ва туғиш бўйича меҳнатга лаёқатсизлик бўйича маълумотлар акс этган. Уларга тўланадиган нафақа фоизи турлича бўлганлиги сабабли дастурда бир ҳужжатнинг ичида ҳисоблаб бўлмайди. Шу сабабли меҳнатга лаёқатсизлик варақасини тақдим этган соғлиқни сақлаш муассасасига мурожаат этинг.");

        var query = Repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            Repository.AddError(
                errorMessage: $"Документ с этим кодом {dto.DocNumber} уже существует",
                propertyNames: nameof(dto.DocNumber));
    }
}
