using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;
//using RestSharp.Extensions;
using SspUis.BizLogicLayer.Info.OrganizationalStructureServices;

namespace SspUis.BizLogicLayer.OrganizationalStructureServices
{
    public static class OrganizationalStructureListDtoSortFilter
    {
        public static IQueryable<OrganizationalStructureListDto> SortFilter(this IQueryable<OrganizationalStructureListDto> query, OrganizationalStructureFilterDto options)
        {
            if (options.HasSearch())
                query = query.Where(a => a.Code.ToLower().Contains(options.Search.ToLower()) ||
                                         a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()));
            if (!string.IsNullOrEmpty(options.Code)) query = query.Where(x => x.Code.Contains(options.Code));

            if (!string.IsNullOrEmpty(options.FullName)) query = query.Where(x => x.Code.Contains(options.FullName));
            if (options.HasSort())
            {
                if (options.SortBy == "code")
                {
                    return options.OrderType.ToLower() == "asc"
                             ? query.OrderBy(a => a.CodeSymbol).ThenBy(a => a.CodeNumber)
                             : query.OrderByDescending(a => a.CodeSymbol).ThenBy(a => a.CodeNumber);
                }
                else
                {
                    query = query.OrderBy($"{options.SortBy} {options.OrderType}");
                }
            }

            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
