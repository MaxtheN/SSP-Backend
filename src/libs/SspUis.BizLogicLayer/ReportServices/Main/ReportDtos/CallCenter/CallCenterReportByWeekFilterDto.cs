using System;

namespace SspUis.BizLogicLayer;

public class CallCenterReportByWeekFilterDto
{
    public bool ByWeek { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    public DateOnly? FromDay { get; set; }
    public DateOnly? ToDay { get; set; }
}
