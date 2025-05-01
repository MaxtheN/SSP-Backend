using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ClaimThemeTranslateDlDto : TranslateDto< ClaimThemeTranslateDlDto,  ClaimThemeTranslate, TranslateColumn>, ILinkToEntity< ClaimThemeTranslate>
    {
    }

    public class  ClaimThemeTranslateDlDtoConfig : PerDtoConfig< ClaimThemeTranslateDlDto,  ClaimThemeTranslate>
    {
        public override Action<IMappingExpression< ClaimThemeTranslate,  ClaimThemeTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ClaimThemeTranslateDlDto,  ClaimThemeTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
