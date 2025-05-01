using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ContractorCategoryCriterionListDtoConfig : PerDtoConfig<ContractorCategoryCriterionListDto, ContractorCategoryCriterion>
{
    public override Action<IMappingExpression<ContractorCategoryCriterion, ContractorCategoryCriterionListDto>> AlterReadMapping =>
        cfg => cfg
         .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent => ent.ContractorCategory.Translates.AsQueryable()
            .FirstOrDefault(ContractorCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ContractorCategory.FullName))
        ;
}
