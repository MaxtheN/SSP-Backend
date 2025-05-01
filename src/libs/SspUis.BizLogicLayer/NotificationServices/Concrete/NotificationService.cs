using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using SspUis.Core;

namespace SspUis.BizLogicLayer.NotificationServices
{
    public class NotificationService
        : BaseEntityService<long, Notification, NotificationListDto, NotificationDto, CreateNotificationDlDto, UpdateNotificationDlDto, INotificationRepository, NotificationSortFilterPageOptions>
        , INotificationService
    {
        private readonly IAuthService _authService;

        public NotificationService(IUnitOfWork unitOfWork, IAuthService authService)
            : base(unitOfWork)
        {
            _authService = authService;
        }

        public HaveId<long> MarkAsRead(long notificationId)
        {
            var not = Repository.ById(notificationId);
            if (not != null)
            {
                var notUser = not.NotificationUsers.FirstOrDefault(a => a.UserId == _authService.User.Id);
                if (notUser != null)
                {
                    notUser.NotificationStatusId = NotificationStatusIdConst.READ;
                    UnitOfWork.Context.Entry(not).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    UnitOfWork.Save();
                }
            }
            return HaveId.Create(notificationId);
        }

        public void MarkAllAsRead()
        {
            var notUsers = UnitOfWork.Context.Set<NotificationUser>().Where(a => a.UserId == _authService.User.Id
                                                                             && a.NotificationStatusId == NotificationStatusIdConst.UNREAD)
                                                                    .ToList();
            foreach (var notUser in notUsers)
            {
                notUser.NotificationStatusId = NotificationStatusIdConst.READ;
                UnitOfWork.Context.Entry(notUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            UnitOfWork.Save();
        }
        public override PagedResult<NotificationListDto> GetList(NotificationSortFilterPageOptions options)
        {
            var result = SortFilter(Repository.ReadAsNoTracked<NotificationListDto>(a => a.NotificationUsers.AsQueryable()
                                                                                          .Any(s => s.UserId == _authService.User.Id
                                                                                               && (options.NotificationStatusId == null
                                                                                                      || s.NotificationStatusId == options.NotificationStatusId))), options)
                                              .AsPagedResult(options);
            return result;
        }

        protected override IQueryable<NotificationListDto> SortFilter(IQueryable<NotificationListDto> query, NotificationSortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }

    }
}
