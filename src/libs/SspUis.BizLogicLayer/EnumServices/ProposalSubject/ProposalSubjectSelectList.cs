using DocumentFormat.OpenXml.Wordprocessing;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Proposal;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ProposalSubjectSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<ProposalSubject> source)
        {
            return new SelectList<int>(source.Select(a => new SelectListItem<int>
            {
                Value = a.Id,
                Text = a.Translates.AsQueryable().FirstOrDefault(ProposalSubjectTranslate.GetExpr(TranslateColumn.short_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? a.ShortName,
                OrderCode = a.Code
            }));
        }
    }
}
