using SspUis.BizLogicLayer.Claim;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Arbitration;

public class ArbitrationDashFilterOptions
{
    public int? RegionId { get; set; } = null;
    public int? DistrictId { get; set; }
}
public class ArbitrationRateFilterOptions : ArbitrationDashFilterOptions
{
    public int ArbitrationTypeId { get; set; }
}

