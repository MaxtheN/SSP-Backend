using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeLeaveOrderDlDto<TDto>:EntityDto<TDto, EmployeeLeaveOrder>
    where TDto : EmployeeLeaveOrderDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public int? EmployeeSickLeaveTypeId { get; set; }
    public int? OrganizationId { get; set; }
    public long? EmployeeSickLeaveId { get; set; }
    public string ConclusionForPrint { get; set; }
    public virtual List<EmployeeLeaveOrderTableDlDto> Tables { get; set; }
    public List<EmployeeLeaveOrderSignerDlDto> Signer { get; set; }
    public virtual List<EmployeeLeaveOrderFileDlDto> Files { get; set; } = new();

    protected override Action<IMappingExpression<TDto, EmployeeLeaveOrder>> AlterMapping =>
        cfg => cfg.ForMember(x => x.Tables, c => c.Ignore())
                  .ForMember(x => x.Signer, c => c.Ignore())
                  .ForMember(x => x.Files, x => x.Ignore());

    public override EmployeeLeaveOrder CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        Signer.AddTo(entity.Signer);
        entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER_IS_WITH_OUT_PAY, Files.Select(a => a.Id).ToList());
        return entity;
    }

    public override void UpdateEntity(EmployeeLeaveOrder entity)
    {
        base.UpdateEntity(entity);
        Tables.ApplyChangesTo<long,EmployeeLeaveOrderTableDlDto,EmployeeLeaveOrderTable>(entity.Tables);
        entity.StatusId = StatusIdConst.MODIFIED;
        Signer.ApplyChangesTo<long, EmployeeLeaveOrderSignerDlDto, EmployeeLeaveOrderSigner>(entity.Signer);
        entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_EMPLOYEE_LEAVE_ORDER_IS_WITH_OUT_PAY, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
    }
}
