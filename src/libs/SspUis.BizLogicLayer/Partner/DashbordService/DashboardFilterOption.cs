using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Partner;

public class DashboardFilterOption
{
    public int? PrtnContractTypeId { get; set; } 
    public int? RegionId { get; set; }
    public bool? HasRegion { get; set; } = false;
    public int? DistrictId { get; set; }
    public bool? HasDistrict { get; set; } = false;
    public int? MfyId { get; set; }
}

public class RegionRateFilterOption : DashboardFilterOption
{
    public int? TableId  { get; set; } = TableIdConst.DOC_PRTN_CERTIFICATE; 
}
