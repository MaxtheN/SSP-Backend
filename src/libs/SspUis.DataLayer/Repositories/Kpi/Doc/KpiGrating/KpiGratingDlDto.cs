using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Kpi;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;
using WEBASE.Attributes;
using AutoMapper;
using SspUis.Core;
using Minio;
using System.Text.Json.Serialization;

namespace SspUis.DataLayer.Repositories.Kpi;

public class KpiGratingDlDto<TDto> : EntityDto<TDto, KpiGrating> where TDto : KpiGratingDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(50)]
    public string DocNumber { get; set; }

    public int OrganizationId { get; set; }
   
    [JsonIgnore]
    public List<KpiGratingIndicatorDlDto> Indicators { get; set; }
    protected override Action<IMappingExpression<TDto, KpiGrating>> AlterMapping =>
      cfg => cfg.ForMember(x => x.Indicators, z => z.Ignore());

    public override KpiGrating CreateEntity()
    {
        var entity = base.CreateEntity();
        Indicators.AddTo(entity.Indicators, (e, d) => d.Tables.AddTo(e.Tables));
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }

    public override void UpdateEntity(KpiGrating entity)
    {
        base.UpdateEntity(entity);
        Indicators.ApplyChangesTo<long, KpiGratingIndicatorDlDto, KpiGratingIndicator>(entity.Indicators, (e, d) => d.Tables.ApplyChangesTo<long, KpiGratingIndicatorTableDlDto, KpiGratingIndicatorTable>(e.Tables));

        entity.StatusId = StatusIdConst.MODIFIED;
    }

}
