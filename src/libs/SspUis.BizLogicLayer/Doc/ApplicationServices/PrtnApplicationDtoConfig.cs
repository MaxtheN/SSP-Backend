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
    public class PrtnApplicationDtoConfig : PerDtoConfig<PrtnApplicationDto, Application>
    {
        public override Action<IMappingExpression<Application, PrtnApplicationDto>> AlterReadMapping =>
             cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.IsRead, x => x.MapFrom(ent => ent.PrtnApplication.IsRead))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
                .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
                .ForMember(x => x.PrtnContractType, x => x.MapFrom(ent => ent.PrtnApplication.PrtnContractType.Translates.AsQueryable()
                    .FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.PrtnContractType.FullName))
                .ForMember(x => x.PrtnContractTypeId, x => x.MapFrom(ent => ent.PrtnApplication.PrtnContractTypeId))
                .ForMember(x => x.NewVacanciesCount, x => x.MapFrom(ent => ent.PrtnApplication.NewVacanciesCount))
                .ForMember(x => x.RegionName, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.DistrictName, x => x.MapFrom(ent => ent.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
                .ForMember(x => x.ContractorDirector,x=> x.MapFrom(ent => ent.Contractor.Director))
                .ForMember(x => x.PrtnContractTypeFrom,x=> x.MapFrom(ent => ent.PrtnApplication.PrtnContractType.EmployeeRangeFrom))
                .ForMember(x => x.PrtnContractTypeTo,x=> x.MapFrom(ent => ent.PrtnApplication.PrtnContractType.EmployeeRangeTo))
                .ForMember(x => x.MfyName, x=> x.MapFrom(ent => ent.PrtnApplication.Mfy.FullName))
                .ForMember(x => x.ChooseLocation, x=> x.MapFrom(ent => ent.PrtnApplication.ChooseLocation))
                .ForMember(x => x.ChoosedRegionId, x=> x.MapFrom(ent => ent.PrtnApplication.ChoosedRegionId))
                .ForMember(x => x.ChoosedDistrictId, x=> x.MapFrom(ent => ent.PrtnApplication.ChoosedDistrictId))
                .ForMember(x => x.RegionName,x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name,ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.ChoosedRegion, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedRegion.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedRegion.FullName))
                .ForMember(x => x.ChoosedDistrict, x => x.MapFrom(ent => ent.PrtnApplication.ChoosedDistrict.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.PrtnApplication.ChoosedDistrict.FullName))
                .ForMember(x => x.PrtnContractId, x=> x.MapFrom(ent => ent.PrtnApplication.Application.PrtnContract.Id))
                .ForMember(x => x.Graphs, x=> x.MapFrom(ent => ent.PrtnApplication.Graphs))
             ;

    }
}
