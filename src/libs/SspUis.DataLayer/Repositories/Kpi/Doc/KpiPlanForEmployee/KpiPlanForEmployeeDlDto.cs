

using System.Collections.Generic;
using System;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.EF;
using System.Text.Json.Serialization;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using SspUis.Core;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiPlanForEmployeeDlDto<TDto> : EntityDto<TDto, KpiPlanForEmployee> where TDto : KpiPlanForEmployeeDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }

    [LocalizedRequired]
    public int OrganizationId { get; set; }

    [JsonIgnore]
    public List<KpiPlanForEmployeeTableDlDto> Tables { get; set; }

    protected override Action<IMappingExpression<TDto, KpiPlanForEmployee>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore());

    public override KpiPlanForEmployee CreateEntity()
    {
        var entity = base.CreateEntity();
        Tables.AddTo(entity.Tables, (e, d) => d.Creates.AddTo(e.Creates));
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }

    public override void UpdateEntity(KpiPlanForEmployee entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long, KpiPlanForEmployeeTableDlDto, KpiPlanForEmployeeTable>(entity.Tables, (e, d) => d.Creates.ApplyChangesTo<long, KpiPlanEmployeeIndicatorCreateDlDto, KpiPlanEmployeeIndicatorCreate>(e.Creates));

        entity.StatusId = StatusIdConst.MODIFIED;
    }

}
