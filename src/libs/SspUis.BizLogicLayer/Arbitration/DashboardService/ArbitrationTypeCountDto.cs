namespace SspUis.BizLogicLayer.Arbitration;

public class ArbitrationTypeCountDto
{
    public long ArbitrationApplicationCount { get; set; }
    public long ArbitrationApplicationNewCount { get; set; }
    public long TotalArbitrationCanceledCount { get; set; }
    public long TotalArbitrationPartiallyAcceptedCount {  get; set; }
    public long TotalArbitrationAcceptedCount { get; set; } 
}

public class ArbitrationRateDto
{
    public int? RegionId {  get; set; } 
    public int? DistrictId { get; set; }
    public string DistrictOrderCode { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }
    public string District {  get; set; }
    public decimal Amount { get; set; }
    public string Name { get; set; }

}
public class DeadlineNearArbitrationApplicationDto
{
    public string? ContractorName { get; set; }
    public long? ContractorId { get; set; }
    public int? ApplicationsCount { get; set; }
    public int? RegionId {  get; set; }
    public int? DistrictId {  get; set; }
}