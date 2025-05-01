using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class MassPlannedCalculationDlDto<TDto> : EntityDto<TDto, MassPlannedCalculation>
    where TDto : MassPlannedCalculationDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedStringLength(600)]
    public string? Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DepartmentId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int CalculationKindId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long OrgSettlementAccountId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int RoundingTypeId { get; set; }
   // public int? OrganizationId { get; set; }
    public bool? IsCancelation { get; set; }
    public decimal? Percentage { get; set; }
    public decimal? Amount { get; set; }
    [LocalizedRequired]
    public DateOnly StartOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? EndOn { get; set; }

    public override MassPlannedCalculation CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }
    public override void UpdateEntity(MassPlannedCalculation entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
