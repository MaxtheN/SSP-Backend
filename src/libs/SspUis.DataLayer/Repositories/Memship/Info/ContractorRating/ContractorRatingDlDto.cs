using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE;
using WEBASE.EF;
using Newtonsoft.Json;

namespace SspUis.DataLayer.Repositories;

public class ContractorRatingDlDto<TDto> :
    EntityDto<TDto, ContractorRating> where TDto :
    ContractorRatingDlDto<TDto>
{
    [LocalizedRequired]
    public string Code { get; set; } 
    
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int ContractorTypeId { get; set; }
    [LocalizedRequired]
    public string OrderCode { get; set; }
    
    [LocalizedRequired]
    public decimal MinimumPercentage { get; set; }
    [LocalizedRequired]
    public decimal MaximumPercentage { get; set; }
    [LocalizedRequired]
    public int Score { get; set; }
    public List<ContractorRatingTranslateDlDto> Translates { get; set; } = new();
    protected override Action<IMappingExpression<TDto, ContractorRating>> AlterMapping => cfg => cfg
            .ForMember(x => x.Translates, x => x.Ignore());

    public override ContractorRating CreateEntity()
    {
        var res = base.CreateEntity();
        Translates.AddByUniqueFKTo(res.Translates);
        return res;
    }
    public override void UpdateEntity(ContractorRating entity)
    {
        base.UpdateEntity(entity);
        Translates.ApplyChangesByUniqueFKTo(entity.Translates);
    }
}
