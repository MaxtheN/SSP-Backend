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
//    public class TagTranslateDlDto : TranslateDto<TagTranslateDlDto, TagTranslate,TranslateColumn>, ILinkToEntity<TagTranslate>
//    {

//    }

//    public class TagTranslateDlDtoConfig :PerDtoConfig<TagTranslateDlDto, TagTranslate>
//    {
//        public override Action<IMappingExpression<TagTranslate, TagTranslateDlDto>> AlterReadMapping =>
//            cfg => cfg
//                .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

//        public override Action<IMappingExpression<TagTranslateDlDto, TagTranslate>> AlterSaveMapping =>
//            cfg => cfg
//                .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
//    }
//}
