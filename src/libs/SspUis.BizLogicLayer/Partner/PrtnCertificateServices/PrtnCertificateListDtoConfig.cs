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

namespace SspUis.BizLogicLayer.PrtnCertificateServices
{
    public class PrtnCertificateListDtoConfig : PerDtoConfig<PrtnCertificateListDto, PrtnCertificate>
    {
        public override Action<IMappingExpression<PrtnCertificate, PrtnCertificateListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Mfy, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.Mfy.FullName))
                .ForMember(x => x.MfyId, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.MfyId))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContractType.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.PrtnApplicationId, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.Id))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.NewVacanciesCount, x => x.MapFrom(ent => ent.PrtnContract.NewVacanciesCount))
                .ForMember(x => x.IsExpired, x => x.MapFrom(ent => ent.ExpireOn < DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(x => x.ContractorRegionId, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.ChooseLocation ? ent.PrtnContract.Application.PrtnApplication.ChoosedRegionId : ent.Contractor.RegionId))
                .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.ChooseLocation ?
                    ent.PrtnContract.Application.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContract.Application.PrtnApplication.ChoosedRegion.FullName
                    : ent.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Region.FullName))
                  .ForMember(x => x.ContractorDistrictId, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.ChooseLocation ? ent.PrtnContract.Application.PrtnApplication.ChoosedDistrictId : ent.Contractor.DistrictId))
                .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.PrtnContract.Application.PrtnApplication.ChooseLocation ?
                    ent.PrtnContract.Application.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnContract.Application.PrtnApplication.ChoosedDistrict.FullName
                    : ent.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.District.FullName))
                .ForMember(x => x.OkedCode, x => x.MapFrom(ent => ent.Contractor.Oked.Code))
                .ForMember(x => x.IsTotalSuccessPost, x => x.MapFrom(ent => ent.TotalPostCount == ent.SuccessPostCount))
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Contractor.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.Oked.FullName))
                ;

    }
}
