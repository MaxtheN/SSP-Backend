using AutoMapper;
using System;
using System.Collections.Generic;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeMissedDayDlDto<TDto> : EntityDto<TDto, EmployeeMissedDay>
    where TDto : EmployeeMissedDayDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocDate { get; set; }
    public string Details { get; set; }
    [LocalizedRequired]
    public bool ForAllEmployee { get; set; }
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public DateOnly? SatrtOn { get; set; }
    public DateOnly? EndOn { get; set; }
    public List<EmployeeMissedDayTableDlDto>? Tables { get; set; }

    protected override Action<IMappingExpression<TDto, EmployeeMissedDay>> AlterMapping => cfg => cfg
        .ForMember(x => x.Tables, x => x.Ignore());

    public override EmployeeMissedDay CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.TableId = TableIdConst.HRM_DOC_MISSED_DAY;
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);

        return entity;
    }

    public override void UpdateEntity(EmployeeMissedDay entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long, EmployeeMissedDayTableDlDto, EmployeeMissedDayTable>(entity.Tables);
    }
}
