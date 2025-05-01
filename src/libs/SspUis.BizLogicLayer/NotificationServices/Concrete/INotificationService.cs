using SspUis.BizLogicLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public interface INotificationService : IBaseEntityService<long, Notification, NotificationListDto, NotificationDto, CreateNotificationDlDto, UpdateNotificationDlDto, NotificationSortFilterPageOptions>
    {
        void MarkAllAsRead();
        HaveId<long> MarkAsRead(long notificationId);
    }
}
