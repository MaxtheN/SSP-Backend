using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Claim;

namespace SspUis.BizLogicLayer.Claim;

public class MediationPlanDtoConfig : PerDtoConfig<MediationPlanDto, MediationPlan>
{
    public override Action<IMappingExpression<MediationPlan, MediationPlanDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
        .ForMember(d => d.MeetingType, c => c.MapFrom(e => e.MeetingType.Translates.AsQueryable()
                .FirstOrDefault(MeetingTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.MeetingType.FullName))
        .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Contractor.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
        .ForMember(d => d.ApplicaionDocNumber, c => c.MapFrom(e => e.Application.DocNumber))
        .ForMember(d => d.ApplicaionDocOn, c => c.MapFrom(e => e.Application.DocOn))
        .ForMember(d => d.Tables, c => c.MapFrom(e => e.Application.ClaimApplication.Tables))
        ;
}
