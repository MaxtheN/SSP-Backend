using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer
{
	public class SrvDeedListDtoConfig : PerDtoConfig<SrvDeedListDto, ServiceDeed>
	{
		public override Action<IMappingExpression<ServiceDeed, SrvDeedListDto>> AlterReadMapping =>
		cfg => cfg
			.ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
				.FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? e.Status.FullName))

			.ForMember(x => x.SrvApplicationOrganizationId, x => x.MapFrom(ent => ent.SrvContract.Application.ServiceApplication.OrganisationId))
			.ForMember(x => x.CanCreatePaymentOrder, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
							? ent.StatusId == StatusIdConst.SIGNED : false))
			.ForMember(x => x.Price, x => x.MapFrom(ent => ent.Groups.SelectMany(a => a.Tables).Sum(b => b.Price)))
			.ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
			.ForMember(x => x.SrvContractStatusId, x => x.MapFrom(ent => ent.SrvContract.StatusId))
			.ForMember(x => x.OrganizationRegion, x => x.MapFrom(ent => ent.Organization.Region.FullName))
			.ForMember(x => x.OrganizationDistrict, x => x.MapFrom(ent => ent.Organization.District.FullName))
			.ForMember(x => x.ContractorFullName, x => x.MapFrom(ent => ent.Contractor.FullName))
			.ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Contractor.Region.FullName))
			.ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Contractor.District.FullName))
			.ForMember(x => x.ContractorDistrictId, x => x.MapFrom(ent => ent.Contractor.District.Id))
			.ForMember(x => x.ContractorRegionId, x => x.MapFrom(ent => ent.Contractor.Region.Id))
			.ForMember(x => x.OrderCode, x => x.MapFrom(ent => ent.Contractor.Region.OrderCode))
			.ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
		//.ForMember(x => x.CompletedWorksCount, x => x.MapFrom(ent => ent.CompletedServices.Count(x => x.StatusId == StatusIdConst.ACCEPTED)))
		;
	}
}
