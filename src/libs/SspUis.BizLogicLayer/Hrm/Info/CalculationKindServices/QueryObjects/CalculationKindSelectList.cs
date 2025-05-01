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

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public static class CalculationKindSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<CalculationKind> query, int? calculationKindId = null)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Where(a => !calculationKindId.HasValue || a.CalculationTypeId == calculationKindId.Value)
                    .Select(x => new SelectListItem<int>
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(CalculationKindTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }
}
