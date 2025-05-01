using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class RecallLeaveDlDto<TDto> : EntityDto<TDto, RecallLeave>
    where TDto : RecallLeaveDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int? OrganizationId { get; set; }

    public List<RecallLeaveTableDlDto> Tables { get; set; }
    public List<RecallLeaveSignerDlDto> Signer { get; set; }
    public virtual List<ReCallLeaveFileDlDto> Files { get; set; }

    protected override Action<IMappingExpression<TDto, RecallLeave>> AlterMapping =>
         cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                   .ForMember(x => x.Signer, c => c.Ignore())
                   .ForMember(x => x.Files, c => c.Ignore());

    public override RecallLeave CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_RE_CALL_LEAVE, Files.Select(a => a.Id).ToList());

        return entity;
    }

    public override void UpdateEntity(RecallLeave entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long, RecallLeaveTableDlDto, RecallLeaveTable>(entity.Tables);
        entity.StatusId = StatusIdConst.MODIFIED;
        Signer.ApplyChangesTo<long, RecallLeaveSignerDlDto, RecallLeaveSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_RE_CALL_LEAVE, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
