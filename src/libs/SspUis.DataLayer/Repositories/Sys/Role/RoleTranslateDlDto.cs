using AutoMapper;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WEBASE;

namespace SspUis.DataLayer.Repositories
{
    public class RoleTranslateDlDto :
       TranslateDto<RoleTranslateDlDto, RoleTranslate, TranslateColumn>,
       ILinkToEntity<RoleTranslate>
    {

    }

    public class RoleTranslateDlDtoConfig : PerDtoConfig<RoleTranslateDlDto, RoleTranslate>
    {
        public override Action<IMappingExpression<RoleTranslate, RoleTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<RoleTranslateDlDto, RoleTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
