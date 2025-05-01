using System;

namespace SspUis.BizLogicLayer;

public class CallCenterReportByWeekDto
{
    public int? WeekDay { get; set; }
    public string? WeekDayName { get; set; }
    public int? UserId { get; set; }
    public string? UserFullName { get; set; }
    public long? Total { get; set; }
    public decimal? TotalPercentage { get; set; }
    public long? InWorkTime { get; set; }
    public decimal? InWorkTimePercentage
    {
        get
        {
            if (InWorkTime.HasValue && Total.HasValue && Total.Value != 0)
            {
                return Math.Round(100m * InWorkTime.Value / Total.Value, 2);
            }
            return 0;
        }
    }
    public long? OutWorkTime { get; set; }
    public decimal? OutWorkTimePercentage
    {
        get
        {
            if (OutWorkTime.HasValue && Total.HasValue && Total.Value != 0)
            {
                return Math.Round(100m * OutWorkTime.Value / Total.Value, 2);
            }
            return 0;
        }
    }
}
