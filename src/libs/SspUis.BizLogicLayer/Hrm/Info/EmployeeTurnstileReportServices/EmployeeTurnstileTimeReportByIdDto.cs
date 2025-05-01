using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileTimeReportByIdDto
{
    public int EmployeeId { get; set; }
    public string EventOn { get; set; }
    public string WeekDay { get; set; }
    public string TotalPeriodTime { get; set; }
    public string TotalPeriodTimeSchedule { get; set; }
    public List<TimeReport> TimeReportList { get; set; } = new();
}

public class TimeReport
{
    public string? EnterAt { get; set; }
    public string? ExitAt { get; set; }
    public int PeriodMinute { get; set; }
    public string? EnterAtSchedule { get; set; }
    public string? ExitAtSchedule { get; set; }
    public int PeriodMinuteSchedule { get; set; }
}
