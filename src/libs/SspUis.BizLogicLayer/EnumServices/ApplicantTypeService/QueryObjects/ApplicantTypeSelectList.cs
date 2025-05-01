using DocumentFormat.OpenXml.Wordprocessing;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ApplicantTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ApplicantType> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(ApplicantTypeTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.ShortName,
                OrderCode = a.Code
            }));
        }
    }
}
