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
    public class CitizenshipTranslateDlDto :
        TranslateDto<CitizenshipTranslateDlDto, CitizenshipTranslate, TranslateColumn>,
        ILinkToEntity<CitizenshipTranslate>
    {

    }
    public class CitizenshipTranslateDlDtoConfig : PerDtoConfig<CitizenshipTranslateDlDto, CitizenshipTranslate>
    {
        public override Action<IMappingExpression<CitizenshipTranslate, CitizenshipTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<CitizenshipTranslateDlDto, CitizenshipTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
