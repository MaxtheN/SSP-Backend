using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public static class OrganizationSettlementAccountSelectList
    {
        public static SelectList<long> AsSelectListItem(this IQueryable<OrganizationSettlementAccount> query)
        {
            return new SelectList<long>
                (
                    query
                    .IsActive().Select(a => new OrganizationSettlementAccountSelectListItem<long>
                    {
                        Value = a.Id,
                        OrderCode = a.AccountCode,
                        Text = a.AccountCode,
                        TextAndCode = a.AccountCode + " - " + a.AccountName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }

        public class OrganizationSettlementAccountSelectListItem<TValue> : SelectListItem<long>
        {
            public string TextAndCode { get; set; }
        }
    }
}
