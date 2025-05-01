using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationDiscussionDlDto<TDto> : EntityDto<TDto, ArbitrationDiscussion>
    where TDto : ArbitrationDiscussionDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateTime DiscussionDate { get; set; }
    [LocalizedRequired]
    public long ArbitrationCourtApplicationId { get; set; }
    //public int StatusId { get; set; }
    public List<ArbitrationDiscussionSignDlDto> Signs { get; set; } = new();
    public List<ArbitrationDiscussionFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, ArbitrationDiscussion>> AlterMapping =>
          cfg => cfg
          .ForMember(x => x.Files, x => x.Ignore())
          .ForMember(x => x.Signs, x => x.Ignore());

    public override ArbitrationDiscussion CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_ARBITRATION_DISCUSSION, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(ArbitrationDiscussion entity)
    {
        base.UpdateEntity(entity);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_ARBITRATION_DISCUSSION, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
