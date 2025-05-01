//using SspUis.DataLayer.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_timesheet_table_day", Schema = "hrm")]
public partial class TimesheetTableDay : IHaveIdProp<long>, IHaveIsDeleted
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("timesheet_indicator_id")]
    public int TimesheetIndicatorId { get; set; }
    [Column("date_on")]
    public DateOnly DateOn { get; set; }
    [Column("plan_days")]
    public int PlanDays { get; set; }
    [Column("plan_hours")]
    public decimal PlanHours { get; set; }
    [Column("fact_days")]
    public int FactDays { get; set; }
    [Column("fact_hours")]
    public decimal FactHours { get; set; }
    [Column("day_off_hours")]
    public decimal DayOffHours { get; set; }
    [Column("night_hours")]
    public decimal NightHours { get; set; }
    [Column("hourly")]
    public decimal? Hourly { get; set; }
    [Column("maintenance_hours")]
    public decimal? MaintenanceHours { get; set; }
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual TimesheetTable Owner { get; set; }
    [ForeignKey(nameof(TimesheetIndicatorId))]
    public virtual TimesheetIndicator TimesheetIndicator { get; set; }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }
}
