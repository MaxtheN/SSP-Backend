using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;

namespace SspUis.BizLogicLayer.DualApplicationServices
{
    public class DualApplicationDtoConfig : PerDtoConfig<DualApplicationDto, DualApplication>
    {
        public override Action<IMappingExpression<DualApplication, DualApplicationDto>> AlterReadMapping =>
            cfg => cfg
               //.ForMember(x => x.Status, x => x.MapFrom(ent => ent.Application.Status.Translates.AsQueryable()
               //     .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Status.FullName))
               // .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Application.Contractor.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Application.Contractor.Inn))
               // .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.Application.ApplicationType.Translates.AsQueryable()
               //     .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.ApplicationType.FullName))
                .ForMember(x => x.DualEducationType, x => x.MapFrom(ent => ent.Application.DualApplication.DualEducationType.Translates.AsQueryable()
                    .FirstOrDefault(DualEducationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.DualApplication.DualEducationType.FullName))
                //.ForMember(x => x.Region, x => x.MapFrom(ent => ent.Application.Region.Translates.AsQueryable().FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.Region.FullName))
                //.ForMember(x => x.District, x => x.MapFrom(ent => ent.Application.District.Translates.AsQueryable().FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Application.District.FullName))
                //.ForMember(x => x.ContractorDirector, x => x.MapFrom(ent => ent.Application.Contractor.Director))
                //.ForMember(x => x.ContractorAddress, x => x.MapFrom(ent => ent.Application.Contractor.Address))
                //.ForMember(x => x.ContractorForm, x => x.MapFrom(ent => ""))
                .ForMember(x => x.DualEducationTypeId, x => x.MapFrom(ent => ent.Application.DualApplication.DualEducationType.Id))
                //.ForMember(x => x.Tables, x => x.MapFrom(ent => ent.Application.DualApplication.Tables))
                ;
    }
}
