using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer;

public static class MemshipCertificateListDtoSortFilter
{
    public static IQueryable<MemshipCertificateListDto> SortFilter(this IQueryable<MemshipCertificateListDto> query
     , MemshipCertificateSortFilterOptions options)
    {
        if (options.IsOld.HasValue && ServiceProvider.AuthService.Contractor == null)
        {
            var dateForNew = new DateOnly(2023, 10, 1);
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            if (options.IsOld == true)
                query = query.Where(a => a.DocOn <= dateForNew && a.ExpireOn > currentDate);
            else
                query = query.Where(a => a.DocOn >= dateForNew);
        }
        if (options.StatusId.HasValue && options.StatusId != 0)
            query = query.Where(d => d.StatusId == options.StatusId);

        if (options.RegionId.HasValue && options.RegionId != 0)
            query = query.Where(d => d.RegionId == options.RegionId);

        if (options.ContractorCategoryId.HasValue && options.ContractorCategoryId != 0)
            query = query.Where(a => a.ContractorCategoryId == options.ContractorCategoryId.Value);

        if (options.DistrictId.HasValue && options.DistrictId != 0)
            query = query.Where(d => d.DistrictId == options.DistrictId);

        if (!options.ContractorInn.IsNullOrEmpty())
            query = query.Where(d => d.ContractorInn == options.ContractorInn || d.ContractorPinfl == options.ContractorInn);

        if (options.IsPinfl.HasValue)
            query = query.Where(d => options.IsPinfl.Value ? d.ContractorPinfl != null : d.ContractorPinfl == null);

		if (options.ContractorOkedId.HasValue)
			query = query.Where(d =>  d.ContractorOkedId == options.ContractorOkedId);

		if (options.MemshipContractTypeId.HasValue)
            query = query.Where(d => d.MemshipContractTypeId == options.MemshipContractTypeId);

        if (options.FromDocDate.HasValue)
            query = query.Where(d => d.DocOn >= options.FromDocDate);

        if (options.ToDocDate.HasValue)
            query = query.Where(d => d.DocOn <= options.ToDocDate);

        if (options.FromExpireOn.HasValue)
            query = query.Where(d => d.ExpireOn >= options.FromExpireOn);

        if (options.ToExpireOn.HasValue)
            query = query.Where(d => d.ExpireOn <= options.ToExpireOn);

        if (options.Less1MonthLeft.HasValue)
        {
            var oneMonthAfter = options.Less1MonthLeft.Value.AddMonths(1);
            query = query.Where(d => d.ExpireOn <= oneMonthAfter && d.ExpireOn >= options.Less1MonthLeft.Value);
        }

        if (options.OpfId.HasValue)
            query = query.Where(d => d.OpfId == options.OpfId);

        if (options.ByExpireOn)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            query = query.Where(d => d.ExpireOn < currentDate);
        }


        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).Contains(options.Search.ToLower()) ||
                                      a.Contractor.Contains(options.Search.ToLower()) ||
                                      a.Organization.Contains(options.Search.ToLower()) ||
                                      a.Director.Contains(options.Search.ToLower()) ||
                                      a.PhoneNumber.Contains(options.Search.ToLower()) ||
                                      a.ContractorSettlementAccount.Contains(options.Search.ToLower()));


        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
