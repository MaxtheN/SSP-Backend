using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationLegalFormTranslateDlDto : TranslateDto<OrganizationLegalFormTranslateDlDto, OrganizationLegalFormTranslate, TranslateColumn>, ILinkToEntity<OrganizationLegalFormTranslate>
    {

    }

    public class OrganizationLegalFormTranslateDlDtoConfig : PerDtoConfig<OrganizationLegalFormTranslateDlDto, OrganizationLegalFormTranslate>
    {
        public override Action<IMappingExpression<OrganizationLegalFormTranslate, OrganizationLegalFormTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<OrganizationLegalFormTranslateDlDto, OrganizationLegalFormTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
