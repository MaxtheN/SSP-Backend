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
    public class RegionTranslateDlDto : 
        TranslateDto<RegionTranslateDlDto, RegionTranslate, TranslateColumn>,
        ILinkToEntity<RegionTranslate>
    {

    }
  
    public class RegionTranslateDlDtoConfig : PerDtoConfig<RegionTranslateDlDto, RegionTranslate>
    {
        public override Action<IMappingExpression<RegionTranslate, RegionTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

        public override Action<IMappingExpression<RegionTranslateDlDto, RegionTranslate>> AlterSaveMapping => 
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
