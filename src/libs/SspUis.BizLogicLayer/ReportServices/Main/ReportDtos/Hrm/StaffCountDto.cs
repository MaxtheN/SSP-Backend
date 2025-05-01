namespace SspUis.BizLogicLayer.ReportServices
{
    public class StaffCountDto
    {
        public int? RegionId { get; set; }
        public string RegionOrderCode { get; set; }
        public string Region { get; set; }

        public int? DepartmentId { get; set; }
        public string DepartmentOrderCode { get; set; }
        public string Department { get; set; }

        public int? PositionId { get; set; }
        public string PositionOrderCode { get; set; }
        public string Position { get; set; }

        public int? OrganizationId { get; set; }
        public string Organization { get; set; }
        public string OrganizationOrderCode { get; set; }

        public decimal? TotalStaffingRate{ get; set; }
        public decimal? TotalEmployeeManageRate { get; set; }
        public decimal? TotalCount { get; set; }
    }
}
