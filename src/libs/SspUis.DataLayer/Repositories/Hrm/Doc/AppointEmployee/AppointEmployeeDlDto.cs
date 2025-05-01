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

public class AppointEmployeeDlDto<TDto> : EntityDto<TDto, AppointEmployee>
    where TDto : AppointEmployeeDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(250)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int? OrganizationId { get; set; }
    public string ConclusionForPrint { get; set; }
    [IgnoreWordProperty]
    public List<AppointEmployeeTableDlDto> Tables { get; set; }
    [IgnoreWordProperty]
    public List<AppointEmployeeSignerDlDto> Signer { get; set; }
    public List<AppointEmployeeFileDlDto> Files { get; set; }

    protected override Action<IMappingExpression<TDto, AppointEmployee>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override AppointEmployee CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_APPOINT_EMPLOYEE_COMMON, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(AppointEmployee entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long, AppointEmployeeTableDlDto, AppointEmployeeTable>(entity.Tables);
        Signer.ApplyChangesTo<long, AppointEmployeeSignerDlDto, AppointEmployeeSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_APPOINT_EMPLOYEE_COMMON, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
