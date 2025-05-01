using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using SspUis.ServiceLayer.NumberServices;
using WEBASE;
using WEBASE.Models;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using Ssp.DataLayer.EFClasses.Edoc;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeMissedDayService : StatusGenericHandler, IEmployeeMissedDayService
{
    private readonly IAuthService _authService;
    private readonly IEmployeeMissedDayRepository _repository;
    private readonly IEmployeeMissedDayRepository _employeeMissedDayRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeManageService _employeeManageService;
    private readonly INumberService _numberService;
    private readonly IDocumentChangeLogService _documentChangeLogService;

    public EmployeeMissedDayService(
        IAuthService authService,
        IEmployeeMissedDayRepository employeeMissedDayRepository,
        IUnitOfWork unitOfWork,
        IEmployeeManageService employeeManageService,
        INumberService numberService,
        IDocumentChangeLogService documentChangeLogService)
    {
        _authService = authService;
        _repository = unitOfWork.EmployeeMissedDayRepository;
        _employeeMissedDayRepository = employeeMissedDayRepository;
        _unitOfWork = unitOfWork;
        _employeeManageService = employeeManageService;
        _numberService = numberService;
        _documentChangeLogService = documentChangeLogService;
    }

    public PagedResult<EmployeeMissedDayListDto> GetList(EmployeeMissedDaySortFilterOptions options)
    {

        var result = GetQuery<EmployeeMissedDayListDto>()
            .SortFilter(options)
            .AsPagedResult(options);

        return result;
    }

    public EmployeeMissedDayDto Get(long id)
    {
        var dto = _employeeMissedDayRepository.ById<EmployeeMissedDayDto>(id);
        CombineStatuses(_employeeMissedDayRepository);

        return dto;
    }
    public EmployeeMissedDayTableDto GetByEmployeeId(long employeeManageId, DateOnly? startOn, DateOnly? endOn)
    {
        if (startOn == null || endOn == null) return null;
        var dto = _employeeMissedDayRepository.ReadAsNoTracked<EmployeeMissedDayDto>().Where(a => a.Tables.Any(t => t.EmployeeManageId == employeeManageId) && a.StatusId == 17).FirstOrDefault();
        var tableItem = dto?.Tables.FirstOrDefault(t => t.EmployeeManageId == employeeManageId && t.StartAt  >= startOn.Value.ToDateTime(TimeOnly.MinValue) && t.EndAt <= endOn.Value.ToDateTime(TimeOnly.MinValue));
        CombineStatuses(_repository);
        return tableItem;
    }

    private IQueryable<TDto> GetQuery<TDto>()
        where TDto : class
    {
        return _employeeMissedDayRepository.ReadAsNoTracked<TDto>();
    }

    public EmployeeMissedDayDto Get()
    {
        var departments = _unitOfWork.Context.Departments.Where(a => a.OrganizationId == _authService.Organization.Id).AsQueryable();

        var orgId = _authService.User.OrganizationId;

        return new EmployeeMissedDayDto
        {
            DocDate = DateExtensions.AsDateOnly(DateTime.Today),
            DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_CONDITATES_COMFIRMED, 1).Item2,
            OrganizationId = orgId,
            DepartmentId = departments.Count() == 1 ? departments.First().Id : 0,
            Department = departments.Count() == 1 ? departments.First().FullName : null,
        };
    }

    public HaveId<long> Create(CreateEmployeeMissedDayDlDto dto, int? organizationId)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            Validate(null, dto);
            if (HasErrors)
                return null;

            try
            {
                var orgId = organizationId != null ? organizationId.Value : _authService.User.OrganizationId;
                var entity = _employeeMissedDayRepository.Create(dto, orgId);

                CombineStatuses(_employeeMissedDayRepository);
                if (HasErrors)
                    return null;

                _unitOfWork.Save();
                if (HasErrors)
                {
                    transaction.Rollback();
                    return null;
                }

                transaction.Commit();
                return HaveId.Create(entity.Id);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public HaveId<long> CreateByEmployee(EmployeeMissedDayTableDlDto dto)
    {
        int? organizationId = _unitOfWork.Context.Set<EmployeeManage>()
            .Include(a => a.Employee)
            .Where(a => a.Id == dto.EmployeeManageId)
            .Select(a => a.Employee.OrganizationId)
            .FirstOrDefault();

        if (organizationId != null)
        {
            var departments = _unitOfWork.Context.Departments
            .Where(a => a.OrganizationId == organizationId)
            .Select(a => a.Id)
            .ToList();

            var orgAreas = _unitOfWork.Context.Organizations
               .Where(a => a.Id == organizationId)
               .Select(a => a.Id)
               .ToList();

            var withoutReasonTypes = new HashSet<int>() { 1, 3, 6, 8, 12, 16, 17 }; // vaqtinchalik hardcode, const ishlatish kerak!!!
            dto.WithoutReason = withoutReasonTypes.Contains(dto.MissedDaysTypeId);

            List<EmployeeMissedDayTableDlDto> tables = new()
            {
                new EmployeeMissedDayTableDlDto
                {
                    EmployeeManageId = dto.EmployeeManageId,
                    MissedDaysTypeId = dto.MissedDaysTypeId,
                    MissedDays = dto.MissedDays,
                    StartAt = dto.StartAt,
                    EndAt = dto.EndAt,
                    WithoutReason = dto.WithoutReason,
                    Details = dto.Details
                }
            };

            var createEntity = new CreateEmployeeMissedDayDlDto
            {
                DocNumber = _numberService.GetNext(NumberTemplateDocumentConst.DOC_CONDITATES_COMFIRMED, 1).Item2,
                DocDate = DateExtensions.AsDateOnly(DateTime.Today),
                ForAllEmployee = false,
                DepartmentId = departments.FirstOrDefault(),
                Tables = tables
            };

            var result = Create(createEntity, organizationId);
            return result;
        }
        return null;
    }

    public HaveId<long> Update(UpdateEmployeeMissedDayDlDto dto)
    {
        var entity = _employeeMissedDayRepository.ById(dto.Id);
        if (entity == null)
        {
            AddError("По вашему запросу запись не найдено");
            return null;
        }

        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                _unitOfWork.Context.EmployeeMissedDays.Lock(dto.Id);
                Validate(entity, dto);
                if (HasErrors)
                    return null;

                _employeeMissedDayRepository.Update(dto);
                CombineStatuses(_employeeMissedDayRepository);
                if (HasErrors)
                    return null;
                _unitOfWork.Save();


                UpdateStatus(new UpdateStatusEmployeeMissedDayDto
                {
                    Id = entity.Id
                }, StatusIdConst.MODIFIED);
                transaction.Commit();

                return HaveId.Create(entity.Id);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    private HaveId<long> CreateDocumentChangeLog(long id, string message = null)
    {
        var moveDto = _repository.ById<EmployeeMissedDayDto>(id, applyFilter: false);
        _documentChangeLogService.Create(
            dto: moveDto,
            tableId: TableIdConst.HRM_DOC_MISSED_DAY,
            organizationId: moveDto.OrganizationId,
            statusId: moveDto.StatusId,
            message: message);
        CombineStatuses(_documentChangeLogService);

        if (HasErrors)
            return null;

        return HaveId.Create(id);
    }

    public HaveId<long> UpdateStatus(UpdateStatusEmployeeMissedDayDto dto, int statusId)
    {
        var updateDto = new UpdateStatusEmployeeMissedDayDlDto
        {
            Id = dto.Id,
            StatusId = statusId
        };

        var canDispose = _unitOfWork.CurrentTransaction == null;
        var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();

        if (HasErrors)
        {
            if (canDispose) transaction.Rollback();
            return null;
        }
        try
        {
            var entity = _employeeMissedDayRepository.UpdateStatus(updateDto);
            CombineStatuses(_employeeMissedDayRepository);

            if (HasErrors)
                return null;

            _unitOfWork.Save();
            var res = CreateDocumentChangeLog(entity.Id);

            if (IsValid && canDispose)
                transaction.Commit();
            return HaveId.Create(entity.Id);
        }
        catch (Exception ex)
        {
            if (canDispose) transaction?.Rollback();
            AddError(ex.Message);
            return null;
        }
        finally
        {
            if (canDispose)
                transaction.Dispose();
        }
    }

    public void Delete(long id)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            try
            {
                _unitOfWork.Context.EmployeeMissedDays.Lock(id);

                var entity = _employeeMissedDayRepository.ById(id);
                UpdateStatus(new UpdateStatusEmployeeMissedDayDto
                {
                    Id = entity.Id
                }, StatusIdConst.MODIFIED);

                if (HasErrors)
                    return;
                _unitOfWork.Save();
                CombineStatuses(_employeeMissedDayRepository);
                if (HasErrors)
                    transaction.Rollback();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public HaveId<long> Approve(UpdateStatusEmployeeMissedDayDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _employeeMissedDayRepository.ById(dto.Id);
            if (entity == null)
            {
                AddError("");
                return null;
            }

            _unitOfWork.Context.EmployeeMissedDays.Lock(dto.Id);
            if (HasErrors)
                return null;

            var res = UpdateStatus(new UpdateStatusEmployeeMissedDayDto
            {
                Id = entity.Id
            }, StatusIdConst.APPROVED);

            transaction.Commit();
            return res;
        }
    }

    public HaveId<long> CancelApprove(UpdateStatusEmployeeMissedDayDto dto)
    {
        using (var transaction = _unitOfWork.BeginTransaction())
        {
            var entity = _employeeMissedDayRepository.ById(dto.Id);

            if (entity == null)
            {
                AddError("");
                return null;
            }

            _unitOfWork.Context.EmployeeMissedDays.Lock(dto.Id);

            if (!StatusIdConst.CanEmployeeMissedDayApplyStatus(entity.StatusId, entity.StatusId))
                _repository.AddError("Нет доступа");

            try
            {
                var result = UpdateStatus(new UpdateStatusEmployeeMissedDayDto
                {
                    Id = entity.Id
                },
                StatusIdConst.REJECTED);
                _unitOfWork.Save();
                transaction.Commit();

                return result;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public List<EmployeeMissedDayInfoDto> GetEmployeeMissedDays(int organizationId, DateOnly startDate, DateOnly endDate)
    {
        var startDateTime = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
        var endDateTime = new DateTime(startDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

        List<EmployeeMissedDayInfoDto> employeeMissedDays = new();

        var missedDays = _unitOfWork.Context.Set<EmployeeMissedDayTable>()
            .Where(a => a.Owner.OrganizationId == organizationId && a.StartAt >= startDateTime && a.EndAt <= endDateTime
                        && a.Owner.StatusId == StatusIdConst.APPROVED && a.WithoutReason == false);

        foreach (var table in missedDays)
        {
            for (DateTime date = table.StartAt.Date; date.Date <= table.EndAt.Date; date = date.AddDays(1))
            {
                var missedDay = new EmployeeMissedDayInfoDto()
                {
                    EmployeeManageId = table.EmployeeManageId,
                    Day = date,
                    StartTime = table.StartAt.TimeOfDay,
                    EndTime = table.EndAt.TimeOfDay
                };
                employeeMissedDays.Add(missedDay);
            }
        }
        return employeeMissedDays;
    }
    private bool Validate<TDto>(EmployeeMissedDay entity, EmployeeMissedDayDlDto<TDto> dto)
        where TDto : EmployeeMissedDayDlDto<TDto>
    {
        var query = _repository.AllAsQueryable;

        if (entity != null)
        {
            query = query.Where(a => a.Id != entity.Id);

            if (!StatusIdConst.CanModifyStatus.Contains(entity.StatusId))
                AddError("Таҳрирлаш мумкин эмас / Невозможно редактировать");
        }

        if (query.ByDocNumber(dto.DocNumber).Any())
            _repository.AddError($"Документ с этим кодом {dto.DocNumber} уже существует", nameof(dto.DocNumber));

        if (!dto.ForAllEmployee && dto.Tables.Count == 0)
            AddError("Table is empty", nameof(dto.Tables));

        if (dto.ForAllEmployee && dto.SatrtOn == null)
        {
            AddError("StartDay not seted");
        }
        if (dto.ForAllEmployee && dto.EndOn == null)
        {
            AddError("EndDay not seted");
        }

        if (dto.SatrtOn >= dto.EndOn)
        {
            AddError("EndDate is earlier than StartDate");
        }

        foreach (var table in dto.Tables)
        {
            if (table.StartAt > table.EndAt)
            {
                AddError("EndOn is earlier than StartOn");
            }
        }

        //if (dto.DocDate < DateOnly.FromDateTime(DateTime.Now.AddDays(_common.CreateDocForPrevDay))
        //    && !_authService.User.Modules.Contains(nameof(ModuleCode.EmployeeComeCreateOnThePastDay)))
        //{
        //    AddError("");
        //    return false;
        //}

        return true;
    }
}
