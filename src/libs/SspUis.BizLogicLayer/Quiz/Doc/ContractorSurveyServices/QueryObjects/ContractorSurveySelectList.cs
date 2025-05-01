using SspUis.DataLayer.EfClasses;
using System.Linq;
using WEBASE.Models;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public static class ContractorSurveytSelectList
{
    public static SelectList<long> AsSelectList(this IQueryable<ContractorSurvey> query)
    {
        return new SelectList<long>(query
            .AsEnumerable()
            .Select(a => new SelectListItem<long>
            {
                Value = a.Id,
                OrderCode = a.DocNumber
            })
            .OrderBy(a => a.OrderCode)
            .ThenBy(a => a.Text));
    }
}

