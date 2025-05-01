 using System;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Corruption;
using System.Linq;

namespace SspUis.BizLogicLayer.Corruption;

public class JoinAntiCorruptionCertificateDtoConfig : PerDtoConfig<JoinAntiCorruptionCertificateDto, JoinAntiCorruptionCertificate>
{
    public override Action<IMappingExpression<JoinAntiCorruptionCertificate, JoinAntiCorruptionCertificateDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(d => d.Status, c => c.MapFrom(e => e.Status.Translates.AsQueryable()
                .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Status.FullName))
            .ForMember(d => d.Organization, c => c.MapFrom(e => e.Organization.Translates.AsQueryable()
                .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? e.Organization.FullName))
            .ForMember(d => d.ContractorInn, c => c.MapFrom(e => e.Contractor.Inn))
            .ForMember(d => d.ContractorDirector, c => c.MapFrom(e => e.Contractor.Director))
            .ForMember(d => d.Contractor, c => c.MapFrom(e => e.Contractor.FullName))

            
            .ForMember(d => d.RegionId, c => c.MapFrom(e => e.Contractor.RegionId))
            .ForMember(d => d.Region, c => c.MapFrom(e => e.Contractor.Region.Translates.AsQueryable()
                .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                .TranslateText ?? e.Contractor.Region.FullName))

            .ForMember(d => d.DistrictId, c => c.MapFrom(e => e.Contractor.DistrictId))
            .ForMember(d => d.District, c => c.MapFrom(e => e.Contractor.District.Translates.AsQueryable()
                .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
            .TranslateText ?? e.Contractor.District.FullName))
        ;
}