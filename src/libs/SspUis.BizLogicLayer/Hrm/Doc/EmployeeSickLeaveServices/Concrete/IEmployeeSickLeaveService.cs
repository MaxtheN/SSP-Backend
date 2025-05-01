using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeSickLeaveService
    : IBaseEntityService<long, EmployeeSickLeave, EmployeeSickLeaveListDto, EmployeeSickLeaveDto, CreateEmployeeSickLeaveDlDto, UpdateEmployeeSickLeaveDlDto,EmployeeSickLeaveSortFilterOptions>
{
    PagedResult<EmployeeSickLeaveListDto> GetList(EmployeeLeaveOrderSortFilter dto);
    //PagedResult<UnpaidEmployeeSickLeaveListDto> GetUnPaidList(UnpaidEmployeeSickLeaveSortFilterPageOptions dto);
    EmployeeSickLeaveDto Get();
    EmployeeSickLeaveDto Get(long id);
    EmployeeSickLeaveTableDto GetByEmployeeId(int emplyeeId);
    //Task<IEnumerable<SickLeaveInfoDto>> GetSickLeaveInfoFromHrMf(string pinfl);
    SelectList<long> AsSelectList(int? employeeId = null);
    HaveId<long> Create(CreateEmployeeSickLeaveDlDto dto);
    void Accept(UpdateStatusEmployeeSickLeaveDto dTo);
    void Cancel(UpdateStatusEmployeeSickLeaveDto dTo);
    void Update(UpdateEmployeeSickLeaveDlDto dto);
    void Delete(long id);
    Task<byte[]> DownloadPdf(Guid id2);
}
