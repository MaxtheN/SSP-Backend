using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices;

public static class ExecutionApplicationListDtoSortFilter
{
    public static IQueryable<ExecutionApplicationListDto> SortFilter(this IQueryable<ExecutionApplicationListDto> query
    , ExecutionApplicationSortFilterOptions options)
    {
        //if (options.StatusId.HasValue)
        //    query = query.Where(d => d.StatusId == options.StatusId);

        //if (options.RegionId.HasValue)
        //    query = query.Where(d => d.RegionId == options.RegionId.Value);

        //if (options.FromDocDate.HasValue)
        //    query = query.Where(d => d.DocOn >= options.FromDocDate);

        //if (options.ToDocDate.HasValue)
        //    query = query.Where(d => d.DocOn <= options.ToDocDate);

        //if (!options.ContractorInn.IsNullOrEmpty())
        //    query = query.Where(d => d.ContractorInn == options.ContractorInn);

        //if (options.DistrictId.HasValue)
        //    query = query.Where(d => d.DistrictId == options.DistrictId);

        //if (options.AppealFormatTypeId.HasValue)
        //    query = query.Where(d => d.AppealFormatTypeId == options.AppealFormatTypeId);

        //if (options.AppealTypeId.HasValue)
        //    query = query.Where(d => d.AppealTypeId == options.AppealTypeId);

        //if (options.AppealDescriptionId.HasValue)
        //    query = query.Where(d => d.AppealDescriptionId == options.AppealDescriptionId);

        //if (options.AppealTypeArriveId.HasValue)
        //    query = query.Where(d => d.AppealTypeArriveId == options.AppealTypeArriveId);

        //if (!options.PersonFullName.IsNullOrEmpty())
        //    query = query.Where(d => d.PersonFullName.ToLower().Contains(options.PersonFullName.ToLower()));



        //if (options.HasSearch())
        //    query = query.Where(a =>
        //                               a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.EdocInfoForList.RegNumber.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.EdocInfoForList.Assignment.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.Contractor.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.PersonName.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.PersonFullName.ToLower().Contains(options.Search.ToLower()) ||
        //                               a.PhoneNumber.ToLower().Contains(options.Search.ToLower()));

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);
        return query;
    }
}
