using System;
using System.Linq;
using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Corruption;
using SspUis.BizLogicLayer.Corruption;

namespace SspUis.BizLogicLayer.HrmCorruption
{
    public class JoinAntiCorruptionCertificateListDtoConfig : PerDtoConfig<JoinAntiCorruptionCertificateListDto, JoinAntiCorruptionCertificate>
    {
        public override Action<IMappingExpression<JoinAntiCorruptionCertificate, JoinAntiCorruptionCertificateListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Status, x => x.MapFrom(ent => ent.Status.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Status.FullName))
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ent.Contractor.Inn))
                .ForMember(x => x.ContractorFullName, x => x.MapFrom(ent => ent.Contractor.FullName))
                .ForMember(x => x.ContractorPhoneNumber, x => x.MapFrom(ent => ent.Contractor.PhoneNumber))

                .ForMember(x => x.ContractorDistrictId, x => x.MapFrom(ent => ent.Contractor.DistrictId))
                .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ent.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Contractor.District.FullName))

                .ForMember(x => x.ContractorRegionId, x => x.MapFrom(ent => ent.Contractor.RegionId))
                .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ent.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Contractor.Region.FullName))
                .ForMember(x => x.Organization, x => x.MapFrom(ent => ent.Organization.Translates.AsQueryable()
                    .FirstOrDefault(OrganizationTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? ent.Organization.FullName))

            //.ForMember(x => x.ApplicationRegionId, x => x.MapFrom(ent => ent.JoinAntiCorruptionResultTable.Application.RegionId))
            //.ForMember(x => x.ApplicationDistrictId, x => x.MapFrom(ent => ent.JoinAntiCorruptionResultTable.Application.DistrictId))
            ;
    }
}


