using AutoMapper;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationTranslateDlDto :
        TranslateDto<OrganizationTranslateDlDto, OrganizationTranslate, TranslateColumn>,
        ILinkToEntity<OrganizationTranslate>
    {
    }

    public class OrganizationTranslateDlDtoConfig : PerDtoConfig<OrganizationTranslateDlDto, OrganizationTranslate>
    {
        public override Action<IMappingExpression<OrganizationTranslate, OrganizationTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<OrganizationTranslateDlDto, OrganizationTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
