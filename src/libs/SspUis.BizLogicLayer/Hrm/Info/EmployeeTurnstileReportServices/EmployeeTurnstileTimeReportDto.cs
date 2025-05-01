using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileTimeReportDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public int? PositionId { get; set; }
    public string PositionName { get; set; }
    public string TotalWorkedTime { get; set; }
    public string TotalScheduledWorkTime { get; set; }
    public List<TurnstileTimeInfo> TurnstileTimeInfos = new();
}

public class TurnstileTimeInfo
{
    public string EventDate { get; set; }
    public string WeekDay { get; set; }
    public string WorkTime { get; set; }
    public string ScheduledWorkTime { get; set; }
    public bool IsLate { get; set; }
    public bool IsLeaveEarly { get; set; }
}
