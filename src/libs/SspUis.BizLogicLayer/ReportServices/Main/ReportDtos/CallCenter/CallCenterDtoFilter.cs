using System;

namespace SspUis.BizLogicLayer;

public class CallCenterDtoFilter
{
    public bool ReportType { get; set; }
    public bool ByRegion { get; set; }
    public int? RegionId { get; set; }
    public bool ByDistrict { get; set; }
    public int? DistrictId { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
}
