using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSickLeaveDlDto<TDto> : EntityDto<TDto, EmployeeSickLeave>
    where TDto : EmployeeSickLeaveDlDto<TDto>
{
    [LocalizedStringLength(30)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } 
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public bool? Checked { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeSickLeaveTypeId { get; set; }
    public int? OrganizationId { get; set; }
    public string ConclusionForPrint { get; set; }
    public List<EmployeeSickLeaveTableDlDto> Tables { get; set; }

    protected override Action<IMappingExpression<TDto, EmployeeSickLeave>> AlterMapping =>
        cfg => cfg.ForMember(d => d.Tables, c => c.Ignore());

    public override EmployeeSickLeave CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        return entity;
    }
    public override void UpdateEntity(EmployeeSickLeave entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long,EmployeeSickLeaveTableDlDto,EmployeeSickLeaveTable>(entity.Tables);
    }
}
