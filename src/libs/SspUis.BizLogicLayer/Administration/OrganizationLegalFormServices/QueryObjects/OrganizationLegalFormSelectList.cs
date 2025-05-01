using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;

namespace SspUis.BizLogicLayer.OrganizationLegalFormServices
{
    public static class OrganizationLegalFormSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<OrganizationLegalForm> query)
        {
            return new SelectList<int>(query
                .IsActive()
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    OrderCode = a.OrderCode,
                    Text = a.Translates.AsQueryable().FirstOrDefault(OrganizationLegalFormTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.FullName
                })
                .OrderBy(a => a.OrderCode)
                .ThenBy(a => a.Text));
        }
    }
}
