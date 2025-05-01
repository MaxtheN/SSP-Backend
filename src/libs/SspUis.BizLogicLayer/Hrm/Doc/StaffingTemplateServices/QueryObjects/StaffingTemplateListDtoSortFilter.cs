using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLayer.Hrm.StaffingTemplateServices
{
    public static class StaffingTemplateListDtoSortFilter
    {
        public static IQueryable<StaffingTemplateListDto> SortFilter(this IQueryable<StaffingTemplateListDto> query, StaffingTemplateListSortFilterDto options)
        {
            if (options.StatusIds != null && options.StatusIds.Count() > 0)
                query = query.Where(a => options.StatusIds.Contains(a.StatusId));

            if (options.HasSearch())
                query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()) ||
                                    a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
                                    a.Status.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
