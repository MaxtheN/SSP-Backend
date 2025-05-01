namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class ClaimApplicationIntegrationForGetDto
    {
        public string ClaimApplicationStatus { get; set; }
        public string? MediationStatus { get; set; }
        public string? MediationPlanStatus { get; set; }
        public string? ApplicationForCourtStatus { get; set; }
        public ClaimApplicationIntegrationStatusIdDto claimApplicationIntegrationStatusId { get; set; } = new();
        public ClaimApplicationIntegrationFileUrlDto claimApplicationIntegrationFileUrl { get; set; } = new();
    }
    public class ClaimApplicationIntegrationStatusIdDto
    {
        public int ClaimApplicationStatusId { get; set; }
        public int? MediationStatusId { get; set; }
        public int? MediationPlanStatusId { get; set; }
        public int? ApplicationForCourtStatusId { get; set; }
    }
    public class ClaimApplicationIntegrationFileUrlDto
    {
        public string ClaimApplicationLink { get; set; }
        public string? MediationPlanLink { get; set; }
        public string? MediationLink { get; set; }
        public string? ApplicationForCourtLink { get; set; }
    }
}
