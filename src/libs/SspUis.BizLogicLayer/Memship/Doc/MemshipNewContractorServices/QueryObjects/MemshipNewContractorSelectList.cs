using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Memship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Memship;



public static class MemshipNewContractorSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<MemshipNewContractor> query)
    {
        return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocNumber
                })
                .OrderBy(a => a.Text)
            );
    }
}