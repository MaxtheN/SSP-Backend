using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.ContractorServices
{
    public static class ContractorListDtoSortFilter
    {
        public static IQueryable<ContractorListDto> SortFilter(this IQueryable<ContractorListDto> query, ISortFilterOptions options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Inn.ToLower().Contains(options.Search.ToLower()) ||
                                         a.InnOrPinfl.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Pinfl.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Oked.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Bank.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Country.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Region.ToLower().Contains(options.Search.ToLower()) ||
                                         a.District.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Address.ToLower().Contains(options.Search.ToLower()) ||
                                         a.State.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
