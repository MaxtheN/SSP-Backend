using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;

namespace SspUis.BizLogicLayer
{
    public class CompletedServiceDtoConfig : PerDtoConfig<CompletedServiceDto, CompletedService>
    {
        public override Action<IMappingExpression<CompletedService, CompletedServiceDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ContractorInn, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Contractor.Inn
                        : string.Empty))

                .ForMember(x => x.ContractorFullName, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Contractor.FullName
                        : string.Empty))

                .ForMember(x => x.ContractorRegion, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Contractor.Region.FullName
                        : string.Empty))

                .ForMember(x => x.ContractorDistrict, x => x.MapFrom(ent => ServiceProvider.AuthService.Contractor != null
                        ? ent.Contractor.District.FullName
                        : string.Empty))

                .ForMember(x => x.OrganizationName, x => x.MapFrom(ent => ent.Organization != null 
                    ? ent.Organization.FullName
                    : string.Empty));
    }
}
