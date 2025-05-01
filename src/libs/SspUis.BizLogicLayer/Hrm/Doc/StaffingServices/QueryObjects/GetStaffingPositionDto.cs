namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class GetStaffingPositionDto
    {
        public int StaffingPositionId { get; set; }
        public decimal EmploymentRate { get; set; }
        public int OrgSettlementAccountId { get; set; }
        public string OrgSettlementAccountCode { get; set; }
    }
}
