using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class SignCriterionDlDto<TDto> : EntityDto<TDto,SignCriterion>
    where TDto : SignCriterionDlDto<TDto>
{
    [LocalizedRequired]
    public DateTime DateOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int ContractorCategoryId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int OrganizationGroupId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public int ApplicationTypeId { get; set; }
    public override SignCriterion CreateEntity()
    {
        var entity = base.CreateEntity();
        return entity;
    }

    public override void UpdateEntity(SignCriterion entity)
    {
        base.UpdateEntity(entity);
    }
}
