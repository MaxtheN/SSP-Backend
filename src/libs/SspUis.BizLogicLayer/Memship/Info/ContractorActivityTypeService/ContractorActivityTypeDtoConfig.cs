using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public class ContractorActivityTypeDtoConfig : PerDtoConfig<ContractorActivityTypeDto, ContractorActivityType>
    {
        public override Action<IMappingExpression<ContractorActivityType, ContractorActivityTypeDto>> AlterReadMapping => cfg => cfg
        .ForMember(x => x.State, x => x.MapFrom(ent =>
                ent.State.Translates.AsQueryable().FirstOrDefault(
                    StateTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.State.FullName))

        .ForMember(x => x.ContractorActivityGroup, x => x.MapFrom(ent =>
                ent.ContractorActivityGroup.Translates.AsQueryable().FirstOrDefault(
                    ContractorActivityGroupTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? ent.ContractorActivityGroup.FullName))
        ;
    }
}
