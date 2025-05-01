using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSendStudyDlDto<TDto> : EntityDto<TDto, EmployeeSendStudy>
    where TDto : EmployeeSendStudyDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string ConclusionForPrint { get; set; }
    public int? OrganizationId { get; set; }
    [IgnoreWordProperty]
    public List<EmployeeSendStudyTableDlDto> Tables { get; set; }
    [IgnoreWordProperty]
    public List<EmployeeSendStudySignerDlDto> Signer { get; set; }
    public virtual List<EmployeeSendStudyFileDlDto> Files { get; set; } = new();


    protected override Action<IMappingExpression<TDto, EmployeeSendStudy>> AlterMapping =>
       cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override EmployeeSendStudy CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER, Files.Select(a => a.Id).ToList());
        return entity;
    }
    public override void UpdateEntity(EmployeeSendStudy entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long, EmployeeSendStudyTableDlDto, EmployeeSendStudyTable>(entity.Tables);
        Signer.ApplyChangesTo<long, EmployeeSendStudySignerDlDto, EmployeeSendStudySigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}