using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.AdditionalAgreementService
{
    public static class AdditionalAgreementSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<AdditionalAgreement> query)
        {
            return new SelectList<long>(
                query
                    .Select(a => new SelectListItem<long>
                    {
                        Value = a.Id
                    })
                    .OrderBy(a => a.Text)
                );
        }
    }
}
