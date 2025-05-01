using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class StateAssetApplicationDtoConfig : PerDtoConfig<StateAssetApplicationDto, Application>
    {
        public override Action<IMappingExpression<Application, StateAssetApplicationDto>> AlterReadMapping =>
             cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
                .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
                .ForMember(x => x.ContractorDirector, x => x.MapFrom(ent => ent.Contractor.Director))
                .ForMember(x => x.ContractorAddress, x => x.MapFrom(ent => ent.Contractor.Address))
                .ForMember(x => x.ContractorForm, x => x.MapFrom(ent => ""))
                .ForMember(x => x.Email, x => x.MapFrom(ent => ent.Contractor.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUserId == ent.CreatedUserId).BusinessmanUser.Email))
                .ForMember(x => x.PhoneNumber, x => x.MapFrom(ent => ent.Contractor.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUserId == ent.CreatedUserId).BusinessmanUser.UserName))
                .ForMember(x => x.RegionSoato, x => x.MapFrom(ent => ent.Region.Soato))
                .ForMember(x => x.DistrictSoato, x => x.MapFrom(ent => ent.District.Soato))
                .ForMember(x => x.PrtnContractTypeId, x => x.MapFrom(ent => ent.StateAssetApplication.PrtnCertificate.PrtnContract.PrtnContractTypeId))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.StateAssetApplication.PrtnCertificate.PrtnContract.PrtnContractType.FullName))
                .ForMember(x => x.PrtnCertificateId2, x => x.MapFrom(ent => ent.StateAssetApplication.PrtnCertificate.Id2))
                .ForMember(x => x.StateAssetStatusId, x => x.MapFrom(ent => ent.StateAssetApplication.StateAssetStatusId))
                .ForMember(x => x.AuctionDocOn, x => x.MapFrom(ent => ent.StateAssetApplication.AuctionDocOn))
                .ForMember(x => x.AuctionDocNumber, x => x.MapFrom(ent => ent.StateAssetApplication.AuctionDocNumber))
                .ForMember(x => x.StateAssetName, x => x.MapFrom(ent => ent.StateAssetApplication.StateAssetName))
             ;

    }
}
