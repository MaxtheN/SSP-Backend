using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetListDto : DocumentListDto<long>, ILinkToEntity<Timesheet>, IHaveIdProp<long>
{
    public int Month { get; set; }
    public DateTime? MonthOn { get; set; }
    public int Year { get; set; }
    public string DocNumber { get; set; }
    public string Department { get; set; }
    public int? DepartmentId { get; set; }
    public string TimesheetType { get; set; }
    public int TimesheetTypeId { get; set; }
    public string Organization { get; set; }
    public string Status { get; set; }
    public string Employees { get; set; }

    #region Actions
    public bool CanModify { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}
