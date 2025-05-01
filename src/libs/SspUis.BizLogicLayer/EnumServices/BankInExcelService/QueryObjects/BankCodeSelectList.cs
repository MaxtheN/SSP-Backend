using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class BankInExcelSelectList
    {
        public static SelectList<long> AsBankSelectList(this IQueryable<Contractor> source)
        {
            var inns = new List<string>() { "207243390", "201055090", "206942764", "207215726", "206916313",
            "200833707", "201055108", "200547792", "200836354", "207127843", "203556638", "201589828",
            "200829053", "201053901", "203709707", "207246047" };

            source = source.Where(a => inns.Contains(a.Inn));
            return new SelectList<long>(source
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.FullName,
                }));
        }
    }
}
