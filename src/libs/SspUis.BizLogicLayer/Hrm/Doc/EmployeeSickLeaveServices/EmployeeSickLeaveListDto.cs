using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeSickLeaveListDto : ILinkToEntity<EmployeeSickLeave>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocOn { get; set; }
    public string Details { get; set; }
    public int StatusId { get; set; }
    public int OrganizationId { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; }
    public string EmployeeNames { get; set; }
    public string EmployeeSickLeaveType { get; set; }
    public int? EmployeeSickLeaveTypeId { get; set; }


    #region Actions
    public bool CanDelete { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
