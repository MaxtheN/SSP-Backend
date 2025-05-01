using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationServices
{
    public class  ClaimOrganizationTranslateDto :  ClaimOrganizationTranslateDlDto, ILinkToEntity< ClaimOrganizationTranslate>
    {
        public string Language { get; set; }
    }

    public class  ClaimOrganizationTranslateDtoConfig : PerDtoConfig< ClaimOrganizationTranslateDto,  ClaimOrganizationTranslate>
    {
        public override Action<IMappingExpression< ClaimOrganizationTranslate,  ClaimOrganizationTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< ClaimOrganizationTranslate,  ClaimOrganizationTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
