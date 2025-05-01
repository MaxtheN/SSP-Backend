using AutoMapper;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public class PrtnContractTypeTableDtoConfig : PerDtoConfig<PrtnContractTypeTableDto, PrtnContractTypeTable>
    {
        public override Action<IMappingExpression<PrtnContractTypeTable, PrtnContractTypeTableDto>> AlterReadMapping =>
           cfg => cfg
            .ForMember(x => x.SignOrganizationType, x => x.MapFrom(ent => ent.SignOrganizationType.Translates.AsQueryable()
                .FirstOrDefault(SignOrganizationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.SignOrganizationType.FullName))
            .ForMember(x => x.Position, x => x.MapFrom(ent => ent.Position.Translates.AsQueryable()
                .FirstOrDefault(PositionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Position.FullName))
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
            .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
           ;
    }
}
