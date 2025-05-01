using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public class ClaimOrganizationDtoConfig : PerDtoConfig<ClaimOrganizationDto, ClaimOrganization>
    {
        public override Action<IMappingExpression<ClaimOrganization, ClaimOrganizationDto>> AlterReadMapping => cfg => cfg
                .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable().FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
        ;
    }
}
