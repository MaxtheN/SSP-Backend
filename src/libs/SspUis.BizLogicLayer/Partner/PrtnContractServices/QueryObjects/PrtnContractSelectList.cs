using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.PrtnContractServices;

namespace SspUis.BizLogicLayer
{
    public static class PrtnContractSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<PrtnContractListDto> query)
        {
            return new SelectList<long>(
                query
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        Text = a.DocNumber+ " - " + a.DocOn
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
