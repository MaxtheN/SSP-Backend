using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveTableDto : EmployeeSickLeaveTableDlDto, ILinkToEntity<EmployeeSickLeaveTable>
{
    public string Employee { get; set; }
    public int EmployeeId { get; set; }
    public string Department { get; set; }
    public int DepartmentId { get; set; }
    public string EmployeeManage { get; set; } = null!;

}
