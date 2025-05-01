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

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public class OrganizationListDtoConfig : PerDtoConfig<OrganizationListDto, Organization>
    {
        public override Action<IMappingExpression<Organization, OrganizationListDto>> AlterReadMapping =>
           cfg => cfg
               .ForMember(x => x.ShortName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ShortName))
               .ForMember(x => x.FullName, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.FullName))
               .ForMember(x => x.Country, x => x.MapFrom(ent => ent.Country.Translates.AsQueryable()
                   .FirstOrDefault(CountryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Country.FullName))
               .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                   .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
               .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                   .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
               .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Oked.Translates.AsQueryable()
                   .FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Oked.FullName))
               .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                   .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
               .ForMember(x => x.Parent, x => x.MapFrom(ent => ent.Parent.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Parent.FullName))
               .ForMember(x => x.SignOrganizationType, x => x.MapFrom(ent => ent.SignOrganizationType.Translates.AsQueryable()
                   .FirstOrDefault(SignOrganizationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.SignOrganizationType.FullName))
               .ForMember(x => x.OrganizationLegalForm, x => x.MapFrom(ent => ent.OrganizationLegalForm.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationLegalFormTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.OrganizationLegalForm.FullName))
           .ForMember(x => x.OrganizationalStructure, x => x.MapFrom(ent => ent.OrganizationalStructure.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationalStructureTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.OrganizationalStructure.FullName))
           .ForMember(x => x.OrganizationGroup, x => x.MapFrom(ent => ent.OrganizationGroup.Translates.AsQueryable()
                   .FirstOrDefault(OrganizationGroupTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.OrganizationGroup.FullName))

            ;
    }
}
