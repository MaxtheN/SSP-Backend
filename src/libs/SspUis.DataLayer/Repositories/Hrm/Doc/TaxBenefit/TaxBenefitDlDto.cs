using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TaxBenefitDlDto<TDto> : EntityDto<TDto, TaxBenefit>
    where TDto : TaxBenefitDlDto<TDto>
{
    [LocalizedRequired]
    public string DocNumber { get; set; } = null!;
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedStringLength(600)]
    public string? Details { get; set; } = null!;
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int EmployeeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int TaxBenefitTypeId { get; set; }
    public DateOnly? EndOn { get; set; }

    public override TaxBenefit CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }
    public override void UpdateEntity(TaxBenefit entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
