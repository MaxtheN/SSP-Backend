using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class TimesheetSortFilterOptions : SortFilterPageOptions
{
    public int? DepartmentId { get; set; }
    public int? TimesheetTypeId { get; set; }
    public string DocNumber { get; set; }
    public int? Id { get; set; }
    public int? StatusId { get; set; }
    public DateOnly? StartOn { get; set; }
    public DateOnly? EndOn { get; set; }
    public DateTime? MonthOn { get; set; }
}
