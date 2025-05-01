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
    public class OkedTranslateDlDto : 
        TranslateDto<OkedTranslateDlDto, OkedTranslate, TranslateColumn>,
        ILinkToEntity<OkedTranslate>
    {

    }
  
    public class OkedTranslateDlDtoConfig : PerDtoConfig<OkedTranslateDlDto, OkedTranslate>
    {
        public override Action<IMappingExpression<OkedTranslate, OkedTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<OkedTranslateDlDto, OkedTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
