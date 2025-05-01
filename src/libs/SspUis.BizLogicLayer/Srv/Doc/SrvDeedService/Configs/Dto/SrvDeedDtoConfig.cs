using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE;

namespace SspUis.BizLogicLayer
{
	public class SrvDeedDtoConfig : PerDtoConfig<SrvDeedDto, ServiceDeed>
	{
		public override Action<IMappingExpression<ServiceDeed, SrvDeedDto>> AlterReadMapping =>
		cfg => cfg
			.ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
				.FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
				.TranslateText ?? e.Status.FullName))
			.ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
			.ForMember(x => x.ContractorFullName, x => x.MapFrom(ent => ent.Contractor.FullName))
			.ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Contractor.Region.FullName))
			.ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Contractor.District.FullName))
			.ForMember(x => x.SrvContractDocNumber, x => x.MapFrom(ent => ent.SrvContract.DocNumber))
			.ForMember(x => x.SrvContractDocOn, x => x.MapFrom(ent => ent.SrvContract.DocOn))
			.ForMember(x => x.OrganizationAddress, x => x.MapFrom(ent => ent.Organization.Address))
			.ForMember(x => x.OrganizationBankAccount, x => x.MapFrom(ent => ent.Organization.SettlementAccounts
					.FirstOrDefault(x => x.StateId == StateIdConst.ACTIVE).AccountCode))
			.ForMember(x => x.OrganizationBankMFO, x => x.MapFrom(ent => ent.Organization.SettlementAccounts
					.FirstOrDefault(x => x.StateId == StateIdConst.ACTIVE).Bank.Code))
			.ForMember(x => x.OrganizationBankName, x => x.MapFrom(ent => ent.Organization.SettlementAccounts
					.FirstOrDefault(x => x.StateId == StateIdConst.ACTIVE).Bank.BankName))
			.ForMember(x => x.OrganizationDirector, x => x.MapFrom(ent => ent.Organization.Director))
			.ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.FullName))
			.ForMember(x => x.OrganizationInn, x => x.MapFrom(ent => ent.Organization.Inn))
			.ForMember(x => x.OrganizationPhoneNumber, x => x.MapFrom(ent => ent.Organization.PhoneNumber))
			////.ForMember(x => x.Region, x => x.MapFrom(ent => ent.Organization.Region))
			.ForMember(x => x.Region, x => x.MapFrom(ent => ent.Organization.Region.Translates.AsQueryable()
																							  .FirstOrDefault(RegionTranslate.GetExpr(DataLayer.TranslateColumn.full_name, 3)).TranslateText ?? ent.Organization.Region.FullName))
		
		    .ForMember(x => x.ContractorAddress, x => x.MapFrom(ent => ent.Contractor.Address))
			.ForMember(x => x.ContractorDirector, x => x.MapFrom(ent => ent.Contractor.Director))
			.ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.Contractor.BusinessmanUserInContractors
					.FirstOrDefault(con => con.StateId == StateIdConst.ACTIVE).BusinessmanUser.UserName))
			.ForMember(x => x.ContractorBankMFO, x => x.MapFrom(ent => ent.Contractor.Bank.Code))
			.ForMember(x => x.ContractorBankAccount, x => x.MapFrom(ent => ent.Contractor.SettlementAccounts
					.FirstOrDefault(con => con.IsMain).AccountCode))
			.ForMember(x => x.ContractorBankName, x => x.MapFrom(ent => ent.Contractor.Bank.BankName))

			.ForMember(x => x.CanSign, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
							? StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.SIGNED)
							: StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.SIGNING)))

			.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
							? StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.REJECTED)
                            : false))
			//.ForMember(x => x.CanReject, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
   //                         ? false
   //                         : StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.REJECTED)))

            .ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
							? false
							: StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.CANCELED)))
            //.ForMember(x => x.CanCancel, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
            //                ? StatusIdConst.CanApplySrvDeedStatus(ent.StatusId, StatusIdConst.CANCELED)
            //                : false))

            .ForMember(x => x.CanCreatePaymentOrder, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
							? ent.StatusId == StatusIdConst.SIGNED
							: false));
	}
}
