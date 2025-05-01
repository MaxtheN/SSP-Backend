using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class KpiRatingEmployeeDlDto<TDto> : EntityDto<TDto, KpiRatingEmployee>
    where TDto : KpiRatingEmployeeDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long PlanForEmployeeId { get; set; }
    public List<KpiRatingEmployeeTableDlDto> Tables { get; set; }
    protected override Action<IMappingExpression<TDto, KpiRatingEmployee>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore());
    public override KpiRatingEmployee CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;

        Tables.AddTo(entity.Tables, (e, d) => d.Points.AddTo(e.Points));
        return entity;
    }
    public override void UpdateEntity(KpiRatingEmployee entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;

        Tables.ApplyChangesTo<long, KpiRatingEmployeeTableDlDto, KpiRatingEmployeeTable>(entity.Tables, (e, d) => d.Points.ApplyChangesTo<long, KpiRatingEmployeePointDlDto, KpiRatingEmployeePoint>(e.Points));
    }
}
