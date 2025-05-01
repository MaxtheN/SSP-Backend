using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Corruption.ContractorUnionActivityTypeServices
{
    public class ContractorUnionActivityTypeDtoConfig : PerDtoConfig<ContractorUnionActivityTypeDto, ContractorUnionActivityType>
    {
        public override Action<IMappingExpression<ContractorUnionActivityType, ContractorUnionActivityTypeDto>> AlterReadMapping => cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
    }
}
