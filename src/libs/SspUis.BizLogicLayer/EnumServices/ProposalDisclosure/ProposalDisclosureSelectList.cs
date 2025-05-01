using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfClasses.Proposal;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ProposalDisclosureSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ProposalDisclosure> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(ProposalDisclosureTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.ShortName,
                OrderCode = a.Code
            }));
        }
    }
}
