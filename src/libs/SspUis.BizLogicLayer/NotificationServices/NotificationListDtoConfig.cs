using GenericServices;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using GenericServices.Configuration;
using AutoMapper;
using WEBASE.Utility;
using SspUis.DataLayer;
using WEBASE;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public class NotificationListDtoConfig : PerDtoConfig<NotificationListDto, Notification>
    {
        public override Action<IMappingExpression<Notification, NotificationListDto>> AlterReadMapping =>
            cfg => cfg
                .ForMember(x => x.Type, x => x.MapFrom(ent => ent.Type.Translates.AsQueryable()
                    .FirstOrDefault(NotificationTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Type.FullName))
                .ForMember(x => x.DocStatus, x => x.MapFrom(ent => ent.DocStatus.Translates.AsQueryable()
                    .FirstOrDefault(StatusTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.DocStatus.FullName))
                .ForMember(x => x.Table, x => x.MapFrom(ent => ent.Table.FullName))
                ;
    }
}
