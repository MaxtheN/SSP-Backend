using SspUis.DataLayer.EfClasses.DualEdu;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public static class InstituteBillingSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<InstituteBilling> query)
    {
        var selectListItems = query
                .Select(x => new SelectListItem<int>
                {
                    Value = x.Id,
                    OrderCode = x.OrderCode,
                    Text = x.FullName,
                })
                .OrderBy(x => x.OrderCode)
                .ToList();

        return new SelectList<int>(selectListItems);
    }
}