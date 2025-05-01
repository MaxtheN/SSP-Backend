namespace SspUis.BizLogicLayer;

public class SrvApplicationTypeDashDto
{
    public long TotalAcceptApplications { get; set; }
    public int TotalAcceptFreeApplications { get; set; }
    public int TotalAcceptPaidApplications { get; set; }
    public int TotalSignedContracts { get; set; }
    public int TotalDeeds { get; set; }
    
    public decimal TotalIncome { get; set; }
    public decimal TotalRemainsIncome { get; set; }
}
