using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.Core;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.MemshipApplicationServices
{
    public class MemshipApplicationDtoConfig : PerDtoConfig<MemshipApplicationDto, MemshipApplication>
    {
        public override Action<IMappingExpression<MemshipApplication, MemshipApplicationDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Oked, x => x.MapFrom(ent => ent.Oked.Translates.AsQueryable().FirstOrDefault(OkedTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Oked.FullName))
                .ForMember(x => x.OkedId, x => x.MapFrom(ent => ent.OkedId))
                .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.IsRead))
                .ForMember(x => x.RegistrationDate, x => x.MapFrom(ent => ent.Application.Contractor.RegistrationDate))
                .ForMember(x => x.OwnerName, x => x.MapFrom(ent => ent.Application.Contractor.OwnerName))
                .ForMember(x => x.ContractorActivityTypeId, x => x.MapFrom(ent => ent.ContractorActivityTypeId))
                .ForMember(x => x.ContractorCategoryId, x => x.MapFrom(ent => ent.ContractorCategoryId))
                .ForMember(x => x.ContractorActivityType, x => x.MapFrom(ent => ent.ContractorActivityType.Translates.AsQueryable()
                    .FirstOrDefault(ContractorActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ContractorActivityType.FullName))
                .ForMember(x => x.ChoosedRegion, x => x.MapFrom(ent => ent.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ChoosedRegion.FullName))
                .ForMember(x => x.ChoosedDistrict, x => x.MapFrom(ent => ent.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ChoosedDistrict.FullName))
                .ForMember(x => x.ContractorCategory, x => x.MapFrom(ent => ent.ContractorCategory.Translates.AsQueryable()
                    .FirstOrDefault(ContractorCategoryTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ContractorCategory.FullName))
                .ForMember(x => x.Files, x => x.MapFrom(ent => ent.Files))
                .ForMember(x => x.CanPaidCancel, x => x.MapFrom(ent => ent.Application.StatusId == StatusIdConst.SENT_FOR_REVIEW && ent.ContractorCategoryId == ContractorCategoryIdConst.YIRIK_KORXONA))
                ;
    }
}
