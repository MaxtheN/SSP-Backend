using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.ContractorServices;

namespace SspUis.BizLogicLayer
{
    public static class ContractorPagedSelectList
    {
        public static PagedSelectList<long> AsPagedSelectList(this PagedResult<ContractorListDto> query)
        {
            return new PagedSelectList<long>(
                query,
                query.Rows
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id,
                        OrderCode = a.Inn,
                        Text = $"{a.Inn} - {a.FullName}"
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
    }
}
