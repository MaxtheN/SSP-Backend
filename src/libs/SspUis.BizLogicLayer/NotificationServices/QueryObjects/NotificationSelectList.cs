using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer
{
    public static class NotificationSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<Notification> query)
        {
            return new SelectList<long>(
                query.Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        OrderCode = a.Id.ToString(),
                        Text = a.Title
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
