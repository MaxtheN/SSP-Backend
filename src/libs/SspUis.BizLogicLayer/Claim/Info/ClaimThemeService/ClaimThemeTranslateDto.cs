using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer.Claim.ClaimThemeServices
{
    public class  ClaimThemeTranslateDto :  ClaimThemeTranslateDlDto, ILinkToEntity< ClaimThemeTranslate>
    {
        public string Language { get; set; }
    }

    public class  ClaimThemeTranslateDtoConfig : PerDtoConfig< ClaimThemeTranslateDto,  ClaimThemeTranslate>
    {
        public override Action<IMappingExpression< ClaimThemeTranslate,  ClaimThemeTranslateDto>> AlterReadMapping =>
            cfg => cfg
                .IncludeBase< ClaimThemeTranslate,  ClaimThemeTranslateDlDto>()
                .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    }
}
