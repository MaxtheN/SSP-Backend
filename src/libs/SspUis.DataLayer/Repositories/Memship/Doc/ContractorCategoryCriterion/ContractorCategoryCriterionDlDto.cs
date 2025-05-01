using System;
using System.Collections.Generic;
using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ContractorCategoryCriterionDlDto<TDto> : EntityDto<TDto, ContractorCategoryCriterion>
    where TDto : ContractorCategoryCriterionDlDto<TDto>
{
    [LocalizedRequired]
    public DateOnly DocOn { get; set; }
    [LocalizedRequired]
    public string DocNumber { get; set; }
    [LocalizedRange(1,int.MaxValue)]
    public int ContractorCategoryId { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public DateOnly? ExpirationDate { get; set; }


    public override ContractorCategoryCriterion CreateEntity()
    {
        var entity= base.CreateEntity();
        entity.StatusId = StatusIdConst.CREATED;
        return entity;
    }
    public override void UpdateEntity(ContractorCategoryCriterion entity)
    {
        base.UpdateEntity(entity);
        entity.StatusId = StatusIdConst.MODIFIED;
    }
}
