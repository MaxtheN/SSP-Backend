using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Keyless]
public class EmployeeTurnstileReportDto
{
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("employee_name")]
    public string EmployeeName { get; set; }
    [Column("position_id")]
    public int? PositionId { get; set; }
    [Column("position_name")]
    public string? PositionName { get; set; }
    [Column("start_on")]
    public string StartOn { get; set; }
    [Column("event_on")]
    public string? EventOn { get; set; }
    [Column("week_day")]
    public string? WeekDay { get; set; }
    [Column("enter_at")]
    public string? EnterAt { get; set; }
    [Column("exit_at")]
    public string? ExitAt { get; set; }
    [Column("enter_at_schedule")]
    public string? EnterAtSchedule { get; set; }
    [Column("exit_at_schedule")]
    public string? ExitAtSchedule { get; set; }
    [Column("period_minute")]
    public int? PeriodMinute { get; set; }
    [Column("period_minute_schedule")]
    public int? PeriodMinuteSchedule { get; set; }
    [Column("period_minute_total")]
    public int? PeriodMinuteTotal { get; set; }
    [Column("enter_count")]
    public long? EnterCount { get; set; }
    [Column("exit_count")]
    public long? ExitCount { get; set;}
}
