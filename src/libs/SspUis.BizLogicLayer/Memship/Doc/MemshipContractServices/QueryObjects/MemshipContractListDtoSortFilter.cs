using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;
using SspUis.Core;

namespace SspUis.BizLogicLayer.Memship;

public static class MemshipContractListDtoSortFilter
{
    public static IQueryable<MemshipContractListDto> SortFilter(this IQueryable<MemshipContractListDto> query
     , MemshipContractSortFilterOptions options)
    {
        
        if (options.IsOld.HasValue)
        {
            var dateForNew = new DateOnly(2023, 10, 1);
            query = query.Where(a => options.IsOld.Value ? a.DocOn <= dateForNew : a.DocOn >= dateForNew);
        }

        if (options.HasSearch())
            query = query.Where(a => ("" + a.DocNumber).ToLower().Contains(options.Search.ToLower()) ||
                                      a.Contractor.ToLower().Contains(options.Search.ToLower()) ||
                                      a.Organization.ToLower().Contains(options.Search.ToLower()) ||
                                      a.Director.ToLower().Contains(options.Search.ToLower()) ||
                                      a.MemshipContractType.ToLower().Contains(options.Search.ToLower()) ||
                                      a.ContractorSettlementAccount.ToLower().Contains(options.Search.ToLower()));

        if (options.StatusId.HasValue && options.StatusId != 0)
            query = query.Where(d => d.StatusId == options.StatusId);

        if (options.ContractorCategoryId.HasValue && options.ContractorCategoryId != 0)
            query = query.Where(a => a.ContractorCategoryId == options.ContractorCategoryId.Value);

        if (options.IsConfirmed.HasValue)
            query = query.Where(d => options.IsConfirmed.Value ? d.StatusId == StatusIdConst.SIGNING : d.StatusId == StatusIdConst.SENT_FOR_REVIEW);

        if (options.HasCertificate.HasValue)
            query = query.Where(d => d.HasCertificate == options.HasCertificate.Value);

        if (options.HasCertificateCanceled == true)
            query = query.Where(d => d.HasCertificateCanceled == true);

        if (options.IsPinfl.HasValue)
            query = query.Where(d => options.IsPinfl.Value ? d.ContractorPinfl != null : d.ContractorPinfl == null);

        if (options.OrganizationId.HasValue)
            query = query.Where(d => d.OrganizationId == options.OrganizationId);

        if (options.RegionId.HasValue && options.RegionId != 0)
            query = query.Where(d => d.RegionId == options.RegionId);

        if (options.DistrictId.HasValue && options.DistrictId != 0)
            query = query.Where(d => d.DistrictId == options.DistrictId);

        if (options.MemshipContractTypeId.HasValue)
            query = query.Where(d => d.MemshipContractTypeId == options.MemshipContractTypeId);

        if (options.MemshipApplicationId.HasValue)
            query = query.Where(d => d.MemshipApplicationId == options.MemshipApplicationId);

        if (!options.ContractorInn.IsNullOrEmpty())
            query = query.Where(d => d.ContractorInn == options.ContractorInn || d.ContractorPinfl == options.ContractorInn);

        if (options.ContractorSettlementAccountId.HasValue)
            query = query.Where(d => d.ContractorSettlementAccountId == options.ContractorSettlementAccountId);

        if (options.OrganizationSettlementAccountId.HasValue)
            query = query.Where(d => d.OrganizationSettlementAccountId == options.OrganizationSettlementAccountId);

        if (options.FromDocDate.HasValue)
            query = query.Where(d => d.DocOn >= options.FromDocDate);

        if (options.ToDocDate.HasValue)
            query = query.Where(d => d.DocOn <= options.ToDocDate);

        if (options.OpfId.HasValue)
            query = query.Where(d => d.OpfId == options.OpfId);

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id);

        return query;
    }
}
