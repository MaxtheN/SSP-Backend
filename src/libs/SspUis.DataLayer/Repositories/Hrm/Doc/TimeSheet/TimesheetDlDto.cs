using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TimesheetDlDto<TDto> : EntityDto<TDto, Timesheet> where TDto : TimesheetDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    public int Month { get; set; }
    [LocalizedRequired]
    public int Year { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int? DepartmentId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int TimesheetTypeId { get; set; }

    protected override Action<IMappingExpression<TDto, Timesheet>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, z => z.Ignore());

    public override Timesheet CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }

    public override void UpdateEntity(Timesheet entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
