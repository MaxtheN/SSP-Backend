using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.Srv.Doc;

public class SrvApplicationTypeCostFilter
{
    public bool? IsFree { get; set; }
    public int? StatusId {  get; set; }
    public int? RegionId { get; set; }
}

public class SrvDashboardFilterOption
{
    public int? RegionId { get; set; }
    public bool? HasRegion { get; set; } = false;
    public int? DistrictId { get; set; }
    public bool? HasDistrict { get; set; } = false;
    public bool? IsFree { get; set; }
    public bool HasWeekly { get; set; } = false;
    public bool HasMonthly { get; set; } = false;
    public bool HasYearly { get; set; } = false;

}

public class SrvRegionRateFilterOptions : SrvDashboardFilterOption
{
    public int? TableId { get; set; } = TableIdConst.DOC_SERVICE_APPLICATION;
}