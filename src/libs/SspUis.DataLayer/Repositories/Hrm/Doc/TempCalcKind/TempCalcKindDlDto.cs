using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TempCalcKindDlDto<TDto>:EntityDto<TDto, TempCalcKind>
    where TDto : TempCalcKindDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string? ConclusionForPrint { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int CalculationKindId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int TempCalcKindTypeId { get; set; }

    public virtual List<TempCalcKindTableDlDto> Tables { get; set; }
    public List<TempCalcKindSignerDlDto> Signer { get; set; }
    public virtual List<TempCalcKindFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, TempCalcKind>> AlterMapping =>
        cfg => cfg.ForMember(d => d.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override TempCalcKind CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_EMPLOYEE_MOD_HELP, Files.Select(x => x.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(TempCalcKind entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long,TempCalcKindTableDlDto,TempCalcKindTable>(entity.Tables);
        Signer.ApplyChangesTo<long, TempCalcKindSignerDlDto, TempCalcKindSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_EMPLOYEE_MOD_HELP, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
