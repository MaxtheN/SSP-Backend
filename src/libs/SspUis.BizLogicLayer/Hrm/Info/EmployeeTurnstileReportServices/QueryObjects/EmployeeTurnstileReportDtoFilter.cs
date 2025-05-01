using System;

namespace SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;

public class EmployeeTurnstileReportDtoFilter
{
    public int? OrganizationId { get; set; }
    public DateTime? OnDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? EnterTime { get; set; }
    public TimeSpan? ExitTime { get; set; }
    public string Employee { get; set; }
    public bool IsMissed { get; set; }
    public bool IsArrived { get; set; }
    public bool IsLate { get; set; }
    public bool IsLeftEarly { get; set; } = false;
    public bool IsBreakLunchTime { get; set; }
}
