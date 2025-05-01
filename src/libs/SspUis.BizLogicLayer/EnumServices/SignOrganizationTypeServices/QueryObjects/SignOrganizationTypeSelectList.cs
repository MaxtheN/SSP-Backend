using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.Core;
using SspUis.BizLogicLayer.ManualServices;

namespace SspUis.BizLogicLayer
{
    public static class SignOrganizationTypeSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<SignOrganizationType> source, SignOrganizationTypeSelectListDtoFilter filter)
        {
            return new SelectList<int>(source
                .Where(a => !filter.IncluceBusinessman.HasValue || a.Id != SignOrganizationTypeIdConst.BUSINESSMAN)
                .Include(a => a.Translates)
                .Select(a => new SelectListItem<int>
                {
                    Value = a.Id,
                    Text = a.Translates.AsQueryable().FirstOrDefault(SignOrganizationTypeTranslate.GetExpr(TranslateColumn.full_name)).TranslateText ?? a.FullName,
                }));
        }
    }
}
