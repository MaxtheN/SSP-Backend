using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class OrganizationalStructureTranslateDlDto : TranslateDto<OrganizationalStructureTranslateDlDto, OrganizationalStructureTranslate, TranslateColumn>,
        ILinkToEntity<OrganizationalStructureTranslate>
    {

    }

    public class OrganizationalStructureTranslateDlDtoConfig : PerDtoConfig<OrganizationalStructureTranslateDlDto, OrganizationalStructureTranslate>
    {
        public override Action<IMappingExpression<OrganizationalStructureTranslate, OrganizationalStructureTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<OrganizationalStructureTranslateDlDto, OrganizationalStructureTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
