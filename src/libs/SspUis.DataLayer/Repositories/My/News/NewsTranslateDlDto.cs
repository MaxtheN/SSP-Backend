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
    public class NewsTranslateDlDto : TranslateDto<NewsTranslateDlDto, NewsTranslate, NewsTranslateColumn>, ILinkToEntity<NewsTranslate>
    {

    }

    public class NewsTranslateDlDtoConfig : PerDtoConfig<NewsTranslateDlDto, NewsTranslate>
    {
        public override Action<IMappingExpression<NewsTranslate, NewsTranslateDlDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<NewsTranslateColumn>()));

        public override Action<IMappingExpression<NewsTranslateDlDto, NewsTranslate>> AlterSaveMapping =>
            cfg => cfg
                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    }
}
