using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using WEBASE;

namespace SspUis.BizLogicLayer.Memship;


public static class ContractorRatingSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<ContractorRating> query)
    {
        return new SelectList<int>(
            query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.Ordercode,
                })
                .OrderBy(a => a.Text)
            );
    }
}