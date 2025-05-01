using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public class EmployeeLeaveOrderSortFilter : SortFilterPageOptions
    {
        public int? EmployeeId { get; set; }
        public int? StatusId { get; set; }
        public bool? IsSelectList { get; set; }
        public int? EmployeeSickLeaveTypeId { get; set; }
    }
}
