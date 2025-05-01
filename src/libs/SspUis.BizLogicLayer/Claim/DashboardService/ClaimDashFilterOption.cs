using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Claim;

public class ClaimDashFilterOption
{
    public int? ClaimApplicationTypeId { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
}

public class ClaimDashRateFilterOption : ClaimDashFilterOption
{
    public int? TableId { get; set; } = TableIdConst.CLAIM__DOC_CLAIM_APPLICATION;
    public bool IsNew { get; set; } = false;
}

public class ClaimContractorTimeLineOption : ClaimDashFilterOption
{
    public bool ByTimeLine { get; set; } = false;
}