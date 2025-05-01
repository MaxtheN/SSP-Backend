using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class  ClaimOrganizationTranslateDlDto : TranslateDto< ClaimOrganizationTranslateDlDto,  ClaimOrganizationTranslate, TranslateColumn>, ILinkToEntity< ClaimOrganizationTranslate>
    {
    }

    public class  ClaimOrganizationTranslateDlDtoConfig : PerDtoConfig< ClaimOrganizationTranslateDlDto,  ClaimOrganizationTranslate>
    {
        public override Action<IMappingExpression< ClaimOrganizationTranslate,  ClaimOrganizationTranslateDlDto>> AlterReadMapping => cfg => cfg
            .ForMember(x => x.ColumnName,x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression< ClaimOrganizationTranslateDlDto,  ClaimOrganizationTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName,x => x.MapFrom(dto => dto.ColumnName));
    }
}
