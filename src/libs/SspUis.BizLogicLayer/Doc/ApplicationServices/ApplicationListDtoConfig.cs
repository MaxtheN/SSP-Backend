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
using SspUis.Core;

namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class ApplicationListDtoConfig : PerDtoConfig<ApplicationListDto, Application>
    {
        public override Action<IMappingExpression<Application, ApplicationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                 .ForMember(x => x.CreatedAt, x => x.MapFrom(ent => ent.CreatedAt))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.IsLastOffer, x => x.MapFrom(ent => ent.Contractor.IsLastOffer))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
                    .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
                .ForMember(x => x.PrtnApplicationNewVacanciesCount, x => x.MapFrom(ent => ent.PrtnApplication.NewVacanciesCount))
                .ForMember(x => x.PrtnContractStatusId, x => x.MapFrom(ent => ent.PrtnContract.StatusId))
                .ForMember(x => x.PrtnContractStatus, x => x.MapFrom(ent => ent.PrtnContract.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContract.Status.FullName))
            .ForMember(x => x.PrtnSertificateStatus, x => x.MapFrom(ent => ent.PrtnContract.PrtnCertificate.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContract.PrtnCertificate.Status.FullName))
                .ForMember(x => x.PrtnContractTypeId, x => x.MapFrom(ent => ent.PrtnApplication.PrtnContractTypeId))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnApplication.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.PrtnContractType.FullName))
                .ForMember(x => x.OrganizationId, x => x.MapFrom(ent => ent.PrtnContract.OrganizationId))
                .ForMember(x => x.OrganizationInn, x => x.MapFrom(ent => ent.PrtnContract.Organization.Inn))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.PrtnContract.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContract.Organization.FullName))
                .ForMember(x => x.ChooseLocation, x => x.MapFrom(ent => ent.PrtnApplication.ChooseLocation))
                .ForMember(x => x.ChoosedRegionId, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedRegionId))
                .ForMember(x => x.ChoosedDistrictId, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedDistrictId))
                .ForMember(x => x.MfyId, x => x.MapFrom(ent => ent.PrtnApplication.MfyId))
                .ForMember(x => x.Mfy, x => x.MapFrom(ent => ent.PrtnApplication.Mfy.FullName))
                .ForMember(x => x.Director, x => x.MapFrom(ent => ent.Contractor.Director))
                .ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.CreatedUser.UserName))
                .ForMember(x => x.ChoosedRegion, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedRegion.FullName))
                .ForMember(x => x.ChoosedDistrict, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedDistrict.FullName))
                .ForMember(x => x.ContractorRegionId, x => x.MapFrom(ent => ent.Contractor.RegionId))
                .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Region.FullName))
                .ForMember(x => x.ContractorDistrictId, x => x.MapFrom(ent => ent.Contractor.DistrictId))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District))
                //.ForMember(x => x.PrtnApplication, x => x.MapFrom(ent => ent.PrtnApplication))
                .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.District.FullName))
                .ForMember(x => x.ContractorHasGovShare, x => x.MapFrom(ent => ent.Contractor.GovShare.HasValue))
                .ForMember(x => x.ContractorGovShare, x => x.MapFrom(ent => ent.Contractor.GovShare))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Contractor.Oked.Code))
                .ForMember(x => x.HasBeenAnswered, x => x.MapFrom(ent => ent.PrtnApplication.HasBeenAnswered))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Oked.FullName))
                .ForMember(x => x.CanViewMahallaResult, x => x.MapFrom(ent => new int[] { StatusIdConst.REJECTED, StatusIdConst.ACCEPTED, StatusIdConst.FULL_FILLED, StatusIdConst.CANCELED }.Contains(ent.StatusId)))
                ;

    }
}
