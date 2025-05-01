using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ApplicationDtoConfig : PerDtoConfig<ApplicationDto, Application>
{
    public override Action<IMappingExpression<Application, ApplicationDto>> AlterReadMapping
        => cfg => cfg
            .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorOpfId, x => x.MapFrom(ent => ent.Contractor.OpfId))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Pinfl ?? ent.Contractor.Inn))
                .ForMember(x => x.ApplicationType, x => x.MapFrom(ent => ent.ApplicationType.Translates.AsQueryable()
                    .FirstOrDefault(ApplicationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.ApplicationType.FullName))
                .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
                .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.District.FullName))
                .ForMember(x => x.ContractorDirector, x => x.MapFrom(ent => ent.Contractor.Director))
                .ForMember(x => x.ContractorAddress, x => x.MapFrom(ent => ent.Contractor.Address))
                .ForMember(x => x.ContractorForm, x => x.MapFrom(ent => ""))
        ;
}

