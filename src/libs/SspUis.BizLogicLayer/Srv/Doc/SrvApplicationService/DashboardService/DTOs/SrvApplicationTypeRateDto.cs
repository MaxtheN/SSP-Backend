namespace SspUis.BizLogicLayer.Srv.Doc;

public class SrvApplicationTypeRateDto
{
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public string DistrictOrderCode { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public decimal Amount { get; set; }
    public string Name { get; set; }
}

public class SrvApplicationStatusDto
{
    public int DocCount { get; set; }
    public int StatusId { get; set; }
    public string? Status { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
}

public class SrvContractStatusDto
{
    public int DocCount { get; set; }
    public int StatusId { get; set; }
    public string? Status { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
}

public class SrvDeedSumDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalRemainsIncome { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public bool? IsFree { get; set; }
}

public class SrvDocumentRegionRate
{
    public int? RegionId { get; set; }
    public string RegionOrderCode { get; set; }
    public string Region { get; set; }
    public int? DistrictId { get; set; }
    public string DistrictOrderCode { get; set; }
    public int DocCount { get; set; } = 0;
    public decimal TotalIncome { get; set; }
    public decimal TotalRemainsIncome { get; set; }
}