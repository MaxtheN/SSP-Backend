

using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;

public static class RatingSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<Rating> source)
    {
        return new SelectList<int>(source.Select(a => new RatingListItem
        {
            Value = a.Id,
            OrderCode = a.OrderCode,
            MinimumPercentage = a.MinimumPercentage,
            MaximumPercentage = a.MaximumPercentage,
            Text = a.Translates.AsQueryable()
            .FirstOrDefault(RatingTranslate.GetExpr(DataLayer.TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName,
        }).OrderBy(a => a.OrderCode).ThenBy(a => a.Text));
    }
    public class RatingListItem : SelectListItem<int>
    {
        public decimal MaximumPercentage { get; set; }
        public decimal MinimumPercentage { get; set; }
    }
}
