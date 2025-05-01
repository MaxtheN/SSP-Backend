namespace SspUis.BizLogicLayer.ClaimApplicationServices
{
    public class DocNumbersByClaimAppTypeDto
    {
        public long Id { get; set; }
        public string DocNumber { get; set; }
    }

    public class ByClaimAppTypeFilter
    {
        public int ContractorId { get; set; }
    }
}
