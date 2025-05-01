using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
//using WEBASE;
//using WEBASE.Attributes;
//using SspUis.DataLayer.EfClasses;

//namespace SspUis.DataLayer.Repositories
//{
//    public class NewsTagTranslateDlDto : TranslateDto<NewsTagTranslateDlDto, NewsTagTranslate,TranslateColumn>, ILinkToEntity<NewsTagTranslate>
//    {

//    }

//    public class NewsTagTranslateDlDtoConfig :PerDtoConfig<NewsTagTranslateDlDto, NewsTagTranslate>
//    {
//        public override Action<IMappingExpression<NewsTagTranslate, NewsTagTranslateDlDto>> AlterReadMapping =>
//            cfg => cfg
//                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

//        public override Action<IMappingExpression<NewsTagTranslateDlDto, NewsTagTranslate>> AlterSaveMapping =>
//            cfg => cfg
//                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
//    }
//}
