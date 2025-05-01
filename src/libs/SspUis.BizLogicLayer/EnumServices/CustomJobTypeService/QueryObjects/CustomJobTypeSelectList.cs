using DocumentFormat.OpenXml.Wordprocessing;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class CustomJobTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<CustomJobType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(CustomJobTypeTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.ShortName,
                OrderCode = a.Code
            }));
        }
    }
}
