using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class DualApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, DualApplication>
    where TDto : DualApplicationDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DualEducationTypeId { get; set; }
    [LocalizedStringLength(1024)]
    public string Message { get; set; }
    public virtual List<DualApplicationTableDlDto> Tables { get; set; }

    protected override Action<IMappingExpression<TDto, DualApplication>> AlterMapping =>
         cfg =>
         {
             base.AlterMapping(cfg);
             cfg.ForMember(x => x.Tables, x => x.Ignore());
         };

    public override DualApplication CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.Application.StatusId = StatusIdConst.CREATED;
        entity.Application.ApplicationTypeId = ApplicationTypeIdConst.DUALEDU;
        entity.Application.Id2 = Guid.NewGuid();
        Tables.AddTo(entity.Tables);
        return entity;
    }

    public override void UpdateEntity(DualApplication entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long, DualApplicationTableDlDto, DualApplicationTable>(entity.Tables);
        entity.Application.StatusId = StatusIdConst.MODIFIED;
    }
}
