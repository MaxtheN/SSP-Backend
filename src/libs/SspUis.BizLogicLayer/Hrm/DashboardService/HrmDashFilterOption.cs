using SspUis.DataLayer;
using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class HrmDashFilterOption
    {
        public int? RegionId { get; set; }
        public int? OrganizationId { get; set; }
        public DateOnly? FromDateTur { get; set; }
        public DateOnly? ToDateTur { get; set; }
    }
    public class StaffingSinglePageReportDtoFilter : DocumentSortFilterOptions
    {
        public int? DepartmentId { get; set; }
        public int? PositionId { get; set; }
        public DateOnly FromDate  { get; set; }
        public DateOnly ToDate { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string Employees { get; set; }
        public int? OrganizationId { get; set; }
    }
}