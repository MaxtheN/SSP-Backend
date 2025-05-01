using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationDelayDlDto<TDto> : EntityDto<TDto, ArbitrationDelay>
    where TDto : ArbitrationDelayDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateTime DelayDate { get; set; }
    [LocalizedRequired]
    public long ArbitrationCourtApplicationId { get; set; }
    //public List<ArbitrationDelaySignDlDto> Signs { get; set; } = new();
    public List<ArbitrationDelayFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, ArbitrationDelay>> AlterMapping =>
          cfg => cfg
          .ForMember(x => x.Files, x => x.Ignore())
          //.ForMember(x => x.Signs, x => x.Ignore())
          ;

    public override ArbitrationDelay CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_ARBITRATION_DELAY, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(ArbitrationDelay entity)
    {
        base.UpdateEntity(entity);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_ARBITRATION_DELAY, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
