using WEBASE;
using WEBASE.Models;
using WEBASE.DependencyInjection;
using System.Linq;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.ContractorActivityGroupServices
{
    public static class ContractorActivityGroupSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ContractorActivityGroup> query)
        {
            return new SelectList<int>(
                query
                    .IsActive()
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.OrderCode,
                        Text = a.Translates.AsQueryable().FirstOrDefault(ContractorActivityGroupTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
