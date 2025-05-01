using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class WorkDayOffDlDto<TDto>:EntityDto<TDto, WorkDayOff>
    where TDto : WorkDayOffDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }

    public virtual List<WorkDayOffTableDlDto> Tables { get; set; }
    public List<WorkDayOffSignerDlDto> Signer { get; set; }

    protected override Action<IMappingExpression<TDto, WorkDayOff>> AlterMapping =>
        cfg => cfg.ForMember(d => d.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore());

    public override WorkDayOff CreateEntity()
    {
        var entity= base.CreateEntity();
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }

    public override void UpdateEntity(WorkDayOff entity)
    {
        Tables.ApplyChangesTo<long,WorkDayOffTableDlDto,WorkDayOffTable>(entity.Tables);
        Signer.ApplyChangesTo<long, WorkDayOffSignerDlDto, WorkDayOffSigner>(entity.Signer);
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
