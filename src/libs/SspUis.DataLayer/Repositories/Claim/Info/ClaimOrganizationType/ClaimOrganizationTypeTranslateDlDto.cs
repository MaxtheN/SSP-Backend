using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ClaimOrganizationTypeTranslateDlDto : TranslateDto< ClaimOrganizationTypeTranslateDlDto,  ClaimOrganizationTypeTranslate, TranslateColumn>, ILinkToEntity< ClaimOrganizationTypeTranslate>
    {
    }

    public class  ClaimOrganizationTypeTranslateDlDtoConfig : PerDtoConfig< ClaimOrganizationTypeTranslateDlDto,  ClaimOrganizationTypeTranslate>
    {
        public override Action<IMappingExpression< ClaimOrganizationTypeTranslate,  ClaimOrganizationTypeTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ClaimOrganizationTypeTranslateDlDto,  ClaimOrganizationTypeTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
