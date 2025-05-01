using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class BaseTimesheetTableDlDto<TDto> : EntityDto<TDto, TimesheetTable>, IHaveIdProp<long>
        where TDto : EntityDto<TDto, TimesheetTable>
{
    public long Id { get; set; }
    public int PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int WorkScheduleId { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int EmploymentTypeId { get; set; }
    public DateOnly? StartOn { get; set; }
    public DateOnly? EndOn { get; set; }
    public decimal EmploymentRate { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int PlanDays { get; set; }
    public decimal PlanHours { get; set; }
    [LocalizedRange(1, int.MaxValue)]
    public int FactDays { get; set; }
    public decimal FactHours { get; set; }
    public decimal DayOffHours { get; set; }
    public decimal NightHours { get; set; }
    public decimal? Hourly { get; set; }
    public decimal? MaintenanceHours { get; set; }
    public decimal? ExperiencePercentage { get; set; }
    public long? DocumentId { get; set; }
    public int? DocumentTableId { get; set; }
    [LocalizedStringLength(300)]
    public string DocumentInfo { get; set; }
    public long OwnerId { get; set; }
    public int DepartmentId { get; set; }
    public int EmployeeId { get; set; }
    public List<TimesheetTableDayDlDto> Days { get; set; }
}

public class TimesheetTableDlDto : BaseTimesheetTableDlDto<TimesheetTableDlDto>
{

}

public class TimesheetTableWithDaysDlDto : BaseTimesheetTableDlDto<TimesheetTableWithDaysDlDto>
{
    public List<TimesheetTableDayDlDto> TableDays { get; set; } = new();

    protected override Action<IMappingExpression<TimesheetTableWithDaysDlDto, TimesheetTable>> AlterMapping => cfg => cfg
        .ForMember(x => x.TableDays, x => x.Ignore());

    public override TimesheetTable CreateEntity()
    {
        var entity = new TimesheetTable();
        Map(entity);
        TableDays.AddTo(entity.TableDays);

        return entity;
    }

    public override void UpdateEntity(TimesheetTable entity)
    {
        Map(entity);
        TableDays.ApplyChangesTo<long, TimesheetTableDayDlDto, TimesheetTableDay>(entity.TableDays);
    }

    private void Map(TimesheetTable entity)
    {
        entity.DayOffHours = TableDays.Sum(x => x.DayOffHours);
        entity.DepartmentId = DepartmentId;
        entity.Details = Details;
        entity.DocumentId = DocumentId;
        entity.DocumentInfo = DocumentInfo;
        entity.DocumentTableId = DocumentTableId;
        entity.EmployeeId = EmployeeId;
        entity.EmployeeManageId = EmployeeManageId;
        entity.EmploymentRate = EmploymentRate;
        entity.EmploymentTypeId = EmploymentTypeId;
        entity.EndOn = EndOn;
        entity.ExperiencePercentage = ExperiencePercentage;
        entity.FactDays = TableDays.Sum(x => x.FactDays);
        entity.FactHours = TableDays.Sum(x => x.FactHours);
        entity.Hourly = Hourly;
        entity.MaintenanceHours = TableDays.Sum(x => x.MaintenanceHours);
        entity.NightHours = TableDays.Sum(x => x.NightHours);
        entity.OwnerId = OwnerId;
        entity.PlanHours = TableDays.Sum(x => x.PlanHours);
        entity.PlanDays = TableDays.Sum(x => x.PlanDays); ;
        entity.PositionId = PositionId;
        entity.StartOn = StartOn;
        entity.WorkScheduleId = WorkScheduleId;
    }
}


