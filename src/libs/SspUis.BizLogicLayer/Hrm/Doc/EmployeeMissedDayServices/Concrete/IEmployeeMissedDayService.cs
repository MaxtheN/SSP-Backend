using StatusGeneric;
using System.Collections.Generic;
using System;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeMissedDayService : IStatusGeneric
{
    HaveId<long> Create(CreateEmployeeMissedDayDlDto dto, int? organizationId);
    HaveId<long> CreateByEmployee(EmployeeMissedDayTableDlDto dto);
    PagedResult<EmployeeMissedDayListDto> GetList(EmployeeMissedDaySortFilterOptions options);
    EmployeeMissedDayDto Get(long id);
    EmployeeMissedDayTableDto GetByEmployeeId(long employeeManageId, DateOnly? startOn, DateOnly? endOn);
    EmployeeMissedDayDto Get();
    List<EmployeeMissedDayInfoDto> GetEmployeeMissedDays(int organizationId, DateOnly startDate, DateOnly endDate);
    HaveId<long> Update(UpdateEmployeeMissedDayDlDto dto);
    HaveId<long> UpdateStatus(UpdateStatusEmployeeMissedDayDto dto, int statusId);
    void Delete(long id);
    HaveId<long> Approve(UpdateStatusEmployeeMissedDayDto dto);
    HaveId<long> CancelApprove(UpdateStatusEmployeeMissedDayDto dto);
}
