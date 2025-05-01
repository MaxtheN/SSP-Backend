using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Keyless]
public class EmployeeTurnstileReportByIdDto
{
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("event_on")]
    public DateTime EventOn { get; set; }
    [Column("week_day")]
    public string? WeekDay { get; set; }
    [Column("enter_at")]
    public string? EnterAt { get; set; }
    [Column("exit_at")]
    public string? ExitAt { get; set; }
    [Column("period_minute")]
    public int PeriodMinute { get; set; }
    [Column("enter_at_schedule")]
    public string? EnterAtSchedule { get; set; }
    [Column("exit_at_schedule")]
    public string? ExitAtSchedule { get; set; }
    [Column("period_minute_schedule")]
    public int PeriodMinuteSchedule { get; set; }
}
