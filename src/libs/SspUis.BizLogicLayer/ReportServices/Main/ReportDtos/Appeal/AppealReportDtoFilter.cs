namespace SspUis.BizLogicLayer;

public class AppealReportDtoFilter
{
    public bool ByRegion { get; set; }
    public int? RegionId { get; set; }
    public bool ByDistrict { get; set; }
    public int? DistrictId { get; set; }
}
