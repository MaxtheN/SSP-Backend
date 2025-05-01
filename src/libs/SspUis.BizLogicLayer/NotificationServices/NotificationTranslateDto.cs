using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.NotificationServices
{
    //public class NotificationTranslateDto : NotificationTranslateDlDto, ILinkToEntity<NotificationTranslate>
    //{
    //    public string Language { get; set; }
    //}

    //public class NotificationTranslateDtoConfig : PerDtoConfig<NotificationTranslateDto, NotificationTranslate>
    //{
    //    public override Action<IMappingExpression<NotificationTranslate, NotificationTranslateDto>> AlterReadMapping =>
    //        cfg => cfg
    //            .IncludeBase<NotificationTranslate, NotificationTranslateDlDto>()
    //            .ForMember(x => x.Language, x => x.MapFrom(ent => ent.Language.FullName));
    //}
}
