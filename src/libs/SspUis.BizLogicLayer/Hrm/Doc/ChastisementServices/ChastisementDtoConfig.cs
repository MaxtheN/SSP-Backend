using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class ChastisementDtoConfig : PerDtoConfig<ChastisementDto, Chastisement>
{
	public override Action<IMappingExpression<Chastisement, ChastisementDto>> AlterReadMapping =>
		cfg => cfg
			.ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable().FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
			.ForMember(d => d.Employees, c => c.MapFrom(ent => ent.Tables.Select(x => x.Employee.Person)))
			.ForMember(d => d.ConclusionForPrint, c => c.MapFrom(ent => ent.ConclusionForPrint))
			.ForMember(d => d.Region, c => c.MapFrom(ent => ent.Organization.Region.Translates.AsQueryable()
			      .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.Region.FullName))
            //.ForMember(d => d.DetailForPrint, c => c.MapFrom(ent => ent.Tables.Select(x => x.DetailForPrint)))
            .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
				.FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Organization.FullName))
			
		  /*  .ForMember(x => x.OrgActivityTypeName, x => x.MapFrom(ent => ent.OrganizationActivityType.Translates.AsQueryable().FirstOrDefault(OrganizationActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.OrganizationActivityType.FullName))*/;
}
