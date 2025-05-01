using System;

namespace SspUis.BizLogicLayer;

public class CallCenterAppealReportByOkedTypeFilter
{
    public bool ByOkedType { get; set; } = false;
    public DateOnly? FromDocOn { get; set; } = null;
    public DateOnly? ToDocOn { get; set; } = null;
}