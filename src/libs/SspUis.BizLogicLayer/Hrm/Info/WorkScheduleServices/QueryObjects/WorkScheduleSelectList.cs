using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.WorkScheduleServices
{
    public static class WorkScheduleSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<WorkSchedule> query, int? workScheduleKindId = null)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Where(a => !workScheduleKindId.HasValue || a.WorkScheduleKindId == workScheduleKindId.Value)
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(WorkScheduleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
