using SspUis.Integration.BankCredit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.ReportServices
{
    public static class BankCreditReportDtoSortFilter
    {
        public static List<BankCreditReportResponse> SortFilter(this List<BankCreditReportResponse> query, BankCreditReportDtoFilter options)
        {
            if (options.RegionId != null && options.RegionId != 0)
            {
                query = query.Where(a=> a.RegionId == options.RegionId).ToList();
            }
            if (options.DistrictId != null && options.DistrictId!= 0)
            {
                query = query.Where(a => a.DistrictId == options.DistrictId).ToList();
            }
            if (options.Year != null)
            {
                query = query.Where(a => a.Year == options.Year).ToList()    ;
            }
            if (options.BankMfo != null)
            {
                query = query.Where(a => a.BankMfo.Intersect(options.BankMfo) != null).ToList();
            }
            return query;
        }
    }
}
