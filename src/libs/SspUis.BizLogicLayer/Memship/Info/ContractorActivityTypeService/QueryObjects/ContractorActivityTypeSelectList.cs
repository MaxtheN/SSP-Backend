using WEBASE;
using WEBASE.Models;
using WEBASE.DependencyInjection;
using System.Linq;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityTypeServices
{
    public static class ContractorActivityTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ContractorActivityType> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(ContractorActivityTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
