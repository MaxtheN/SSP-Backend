using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TimesheetTableDayDlDto : EntityDto<TimesheetTableDayDlDto, TimesheetTableDay>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int TimesheetIndicatorId { get; set; }
    public DateOnly DateOn { get; set; }
    public int PlanDays { get; set; }
    public decimal PlanHours { get; set; }
    public int FactDays { get; set; }
    public decimal FactHours { get; set; }
    public decimal DayOffHours { get; set; }
    public decimal NightHours { get; set; }
    public decimal? Hourly { get; set; }
    public decimal? MaintenanceHours { get; set; }
    public long OwnerId { get; set; }

    public override TimesheetTableDay CreateEntity()
    {
        var entity = new TimesheetTableDay();
        UpdateEntity(entity);
        return entity;
    }

    public override void UpdateEntity(TimesheetTableDay entity)
    {
        entity.DateOn = DateOn;
        entity.DayOffHours = DayOffHours;
        entity.FactDays = FactDays;
        entity.FactHours = FactHours;
        entity.Hourly = Hourly;
        entity.MaintenanceHours = MaintenanceHours;
        entity.NightHours = NightHours;
        entity.OwnerId = OwnerId;
        entity.PlanDays = PlanDays;
        entity.PlanHours = PlanHours;
        entity.TimesheetIndicatorId = TimesheetIndicatorId;
    }
}
