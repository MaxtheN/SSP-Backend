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

namespace SspUis.BizLogicLayer.PrtnContractTypeServices
{
    public static class PrtnContractTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<PrtnContractType> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(x => new PrtnContractTypeSelectListItem
                    {
                        Value = x.Id,
                        OrderCode = x.OrderCode,
                        Text = x.Translates.AsQueryable().FirstOrDefault(PrtnContractTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? x.FullName,
                        EmployeeRangeFrom = x.EmployeeRangeFrom,
                        EmployeeRangeTo = x.EmployeeRangeTo
                    })
                    .OrderBy(x => x.OrderCode)
                    .ThenBy(x => x.Text)
                );
        }
    }

    public class PrtnContractTypeSelectListItem : SelectListItem<int>
    {
        public int EmployeeRangeFrom { get; set; }
        public int? EmployeeRangeTo { get; set; }
    }
}
