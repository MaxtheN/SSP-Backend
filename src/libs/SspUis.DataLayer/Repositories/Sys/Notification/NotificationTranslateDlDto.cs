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
    //public class NotificationTranslateDlDto : 
    //    TranslateDto<NotificationTranslateDlDto, NotificationTranslate, TranslateColumn>,
    //    ILinkToEntity<NotificationTranslate>
    //{

    //}
  
    //public class NotificationTranslateDlDtoConfig : PerDtoConfig<NotificationTranslateDlDto, NotificationTranslate>
    //{
    //    public override Action<IMappingExpression<NotificationTranslate, NotificationTranslateDlDto>> AlterReadMapping =>
    //        cfg => cfg
    //            .ForMember(x => x.ColumnName, x => x.MapFrom(ent => ent.ColumnName.AsEnum<TranslateColumn>()));

    //    public override Action<IMappingExpression<NotificationTranslateDlDto, NotificationTranslate>> AlterSaveMapping => 
    //        cfg => cfg
    //            .ForMember(x => x.ColumnName, x => x.MapFrom(dto => dto.ColumnName));
    //}
}
