using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class PlannedCalculationDlDto<TDto> : EntityDto<TDto, PlannedCalculation>
    where TDto : PlannedCalculationDlDto<TDto>
{
    [LocalizedRequired]
    [LocalizedStringLength(30)]
    public string DocNumber { get; set; }
    [LocalizedRequired]
    public DateOnly DocOn { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int CalculationKindId { get; set; }
    [LocalizedRequired]
    public bool IsCancelation { get; set; }
    public int? OrganizationId { get; set; }

    public List<PlannedCalculationTableDlDto> Tables { get; set; }

    protected override Action<IMappingExpression<TDto, PlannedCalculation>> AlterMapping =>
        cfg => cfg.ForMember(d => d.Tables, c => c.Ignore());

    public override PlannedCalculation CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        Tables.AddTo(entity.Tables);
        return entity;
    }
    public override void UpdateEntity(PlannedCalculation entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
        Tables.ApplyChangesTo<long,PlannedCalculationTableDlDto,PlannedCalculationTable>(entity.Tables);
    }
}
