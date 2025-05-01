using System.Linq;
using SspUis.DataLayer.EfClasses;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class ArbitrationJudgeSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<ArbitrationJudge> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.FirstName,
                    Text = a.LastName + " " + a.FirstName + " " + a.MiddleName
                })
                .OrderBy(a => a.Text)
            );
    }
}
