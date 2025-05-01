using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderSortFilterOptions : SortFilterPageOptions
{
    public int? StatusId { get; set; }
    public int? EmployeeSickLeaveTypeId { get; set; }
}
