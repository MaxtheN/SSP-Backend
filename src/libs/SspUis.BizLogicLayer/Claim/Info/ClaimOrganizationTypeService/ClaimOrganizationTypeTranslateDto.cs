using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Claim.ClaimOrganizationTypeServices
{
    public class  ClaimOrganizationTypeTranslateDto :  ClaimOrganizationTypeTranslateDlDto, ILinkToEntity< ClaimOrganizationTypeTranslate>
    {
        public string Language { get; set; }
    }

    public class  ClaimOrganizationTypeTranslateDtoConfig : PerDtoConfig< ClaimOrganizationTypeTranslateDto,  ClaimOrganizationTypeTranslate>
    {
        public override Action<IMappingExpression< ClaimOrganizationTypeTranslate,  ClaimOrganizationTypeTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< ClaimOrganizationTypeTranslate,  ClaimOrganizationTypeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
