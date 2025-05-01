using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;

public static class TariffScaleCoefSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<TariffScaleCoef> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.TariffScale.Translates.AsQueryable().FirstOrDefault(TariffScaleTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.TariffScale.FullName
                })
                .OrderBy(a => a.Text)
            );
    }
}