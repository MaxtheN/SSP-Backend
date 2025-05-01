using System.Collections.Generic;
using System.Linq;

namespace SspUis.BizLogicLayer;

public class CallCenterDto
{
    public int? RegionId { get; set; }
    public string Region { get; set; }
    public string RegionOrderCode { get; set; }
    public int? DistrictId { get; set; }
    public string District { get; set; }
    public string DistrictOrderCode { get; set; }

    public long? TotalCallCenter { get; set; }

    public long? TotalCallCenterAppeal { get; set; }
    public decimal? TotalCallCenterAppealPercent { get; set; }

    #region First Type Model
    public long? TotalPhysicalCount { get; set; }
    public decimal? TotalPhysicalCountPercent { get; set; }

    public long? TotalLegalCount { get; set; }
    public decimal? TotalLegalCountPercent { get; set; }

    public long? TotalInvestorCount { get; set; }
    public decimal? TotalInvestorPercent { get; set; }

    public long? TotalPhysicalContractorCount { get; set; }
    public decimal? TotalPhysicalContractorPercent { get; set; }
    #endregion

    #region Second Type Model
    public long? TotalMikroContractorCount { get; set; }
    public decimal? TotalMikroContractorCountPercent { get; set; }

    public long? TotalLitteContractorCount { get; set; }
    public decimal? TotalLittleContractorCountPercent { get; set; }

    public long? TotalMiddleContractorCount { get; set; }
    public decimal? TotalMiddleContractorCountPercent { get; set; }

    public long? TotalHigheContractorCount { get; set; }
    public decimal? TotalHigheContractorCountPercent { get; set; }
    #endregion
}
