using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderListDto : DocumentListDto<long>, ILinkToEntity<EmployeeLeaveOrder>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; } 
    public string Details { get; set; }
    public string Employee { get; set; }
    public bool IsWithOutPay { get; set; }
    public string? EmployeeSickLeaveType { get; set; }
    public int? EmployeeSickLeaveTypeId { get; set; }

    #region Actions
    public bool CanDelete { get; set; }
    public bool CanSign { get; set; }
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    #endregion
}
