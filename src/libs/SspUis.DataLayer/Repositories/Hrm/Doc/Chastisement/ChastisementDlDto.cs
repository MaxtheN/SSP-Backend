using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class ChastisementDlDto<TDto> : EntityDto<TDto, Chastisement>
    where TDto : ChastisementDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string ConclusionForPrint { get; set; }
    public int? OrganizationId { get; set; }
    public virtual List<ChastisementTableDlDto> Tables { get; set; } = new();
    public virtual List<ChastisementSignerDlDto> Signer { get; set; } = new();
    public virtual List<ChastisementFileDlDto> Files{ get; set; } = new();

    protected override Action<IMappingExpression<TDto, Chastisement>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override Chastisement CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_CHASTISEMENT, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(Chastisement entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long, ChastisementTableDlDto, ChastisementTable>(entity.Tables);
        Signer.ApplyChangesTo<long, ChastisementSignerDlDto, ChastisementSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_CHASTISEMENT, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
