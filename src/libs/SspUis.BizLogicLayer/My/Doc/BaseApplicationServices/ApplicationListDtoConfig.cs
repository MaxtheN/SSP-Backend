using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer;

public class ApplicationListDtoConfig : PerDtoConfig<ApplicationListDto, Application>
{
    public override Action<IMappingExpression<Application, ApplicationListDto>> AlterReadMapping =>
        cfg => cfg
        .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
            .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
        .ForMember(x => x.Step, x => x.MapFrom(ent => ent.CurrentStep.Translates.AsQueryable()
            .FirstOrDefault(ApplicationTypeStepTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.CurrentStep.FullName))
        .ForMember(x => x.Contractor, x => x.MapFrom(ent => ent.Contractor.FullName))
        .ForMember(x => x.CreatedAt, x => x.MapFrom(ent => ent.CreatedAt))
        .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
        .ForMember(x => x.ContractorPinfl, x => x.MapFrom(ent => ent.Contractor.Pinfl))
        .ForMember(x => x.ContractorDirector, x => x.MapFrom(ent => ent.Contractor.Director))
        .ForMember(x => x.ContractorAdress, x => x.MapFrom(ent => ent.Contractor.Address))
        .ForMember(x=>x.Message, x=>x.MapFrom(ent=>ent.Message))
        .ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.Contractor.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUserId == ent.CreatedUserId).BusinessmanUser.UserName))
        .ForMember(x => x.Region, x => x.MapFrom(ent => ent.Region.Translates.AsQueryable()
            .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Region.FullName))
        .ForMember(x => x.District, x => x.MapFrom(ent => ent.District.Translates.AsQueryable()
        .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Contractor.FullName));

        
}
