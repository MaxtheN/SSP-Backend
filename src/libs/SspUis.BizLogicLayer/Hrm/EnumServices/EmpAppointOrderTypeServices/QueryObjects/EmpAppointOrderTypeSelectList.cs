using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm
{
    public static class EmpAppointOrderTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<EmpAppointOrderType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                OrderCode = a.OrderCode,
                Text = a.Translates.AsQueryable()
                .FirstOrDefault(EmpAppointOrderTypeTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
            }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
        }
    }
}
