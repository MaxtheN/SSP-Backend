using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public class EmployeeManageSortFilterPageOptions : SortFilterPageOptions
    {
        public int? EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public int? OrganizationId { get; set; }
        public bool IsOnlyWorkingEmployee { get; set; } = true;
    }
}
