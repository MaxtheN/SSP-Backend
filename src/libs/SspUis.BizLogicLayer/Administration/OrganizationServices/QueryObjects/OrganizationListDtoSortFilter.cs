using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer.OrganizationServices
{
    public static class OrganizationListDtoSortFilter
    {
        public static IQueryable<OrganizationListDto> SortFilter(this IQueryable<OrganizationListDto> query, OrganizationSortFilterPageOptions options)
        {
            if (options.RegionId.HasValue)
                query = query.Where(a => a.RegionId == options.RegionId.Value);

            if (options.DistrictId.HasValue)
                query = query.Where(a => a.DistrictId == options.DistrictId.Value);

            if (options.SignOrganizationTypeId.HasValue)
                query = query.Where(a => a.SignOrganizationTypeId == options.SignOrganizationTypeId.Value);

            if (options.OrganizationalStructureId.HasValue)
                query = query.Where(a => a.OrganizationalStructureId == options.OrganizationalStructureId.Value);
			if (options.OrganizationGroupId.HasValue)
				query = query.Where(a => a.OrganizationGroupId == options.OrganizationGroupId.Value);

			if (options.HasSearch())
                query = query.Where(a => a.ShortName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.FullName.ToLower().Contains(options.Search.ToLower()) ||
                                         a.Inn.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
