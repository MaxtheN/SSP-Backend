using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ArbitrationResultDlDto<TDto> :
    EntityDto<TDto, ArbitrationResult>
    where TDto : ArbitrationResultDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public decimal PayedAmount { get; set; }
	public bool CanByDivided { get; set; }
	[LocalizedRequired]
    public long ArbitrationCourtApplicationId { get; set; }
    public List<ArbitrationResultSignDlDto> Signs { get; set; } = new();
    public List<ArbitrationResultFileDlDto> Files { get; set; } = new();

	protected override Action<IMappingExpression<TDto, ArbitrationResult>> AlterMapping =>
        cfg => cfg
        .ForMember(x => x.Files, c => c.Ignore())
        .ForMember(x => x.Signs, c => c.Ignore());

    public override ArbitrationResult CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_ARBITRATION_RESULT
            , Files.Select(x => x.Id).ToList());
        Signs.AddTo(entity.Signs);
        return entity;
    }

    public override void UpdateEntity(ArbitrationResult entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_ARBITRATION_RESULT
            , entity.Id.ToString()
            , Files.Select(x => x.Id).ToList());

        Signs.ApplyChangesTo<long, ArbitrationResultSignDlDto, ArbitrationResultSign>(entity.Signs);
    }
}
