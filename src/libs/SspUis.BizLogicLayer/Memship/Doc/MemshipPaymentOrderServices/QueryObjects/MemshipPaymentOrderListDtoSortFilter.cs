using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Memship;

public static class MemshipPaymentOrderListDtoSortFilter
{
    public static IQueryable<MemshipPaymentOrderListDto> SortFilter(this IQueryable<MemshipPaymentOrderListDto> query, MemshipPaymentOrderSortFilterOption options)
    {
        if (options.CurrencyId.HasValue)
            query = query.Where(x => x.CurrencyId == options.CurrencyId);

        if (options.OrganizationId.HasValue)
            query = query.Where(x => x.OrganizationId == options.OrganizationId);

        if (options.StatusId.HasValue)
            query = query.Where(x => x.StatusId == options.StatusId);

        if (options.BankId.HasValue)
            query = query.Where(x => x.BankId == options.BankId);

        if (options.MemshipContractId.HasValue)
            query = query.Where(x => x.MemshipContractId == options.MemshipContractId);

        if (options.ContractorId.HasValue)
            query = query.Where(x => x.ContractorId == options.ContractorId);

        if (options.FromDocOn.HasValue)
            query = query.Where(x => x.DocOn >= options.FromDocOn);

        if (options.ToDocOn.HasValue)
            query = query.Where(x => x.DocOn <= options.ToDocOn);

        if (options.ApplicationTypeId.HasValue)
            query = query.Where(x => x.ApplicationTypeId == options.ApplicationTypeId);

        if (options.ServiceContractId.HasValue)
            query = query.Where(x => x.ServiceContractId == options.ServiceContractId);


        if (options.HasSearch())
            query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower()) ||
                                     a.ContractorInn.ToLower().Contains(options.Search.ToLower()) ||
                                     a.Amount.ToString().ToLower().Contains(options.Search.ToLower()) //||
                                     //a.BankName.ToLower().Contains(options.Search.ToLower()) ||
                                     //a.BankCode.ToLower().Contains(options.Search.ToLower()) ||
                                     //a.Currency.ToLower().Contains(options.Search.ToLower()) ||
                                     //a.Organization.ToLower().Contains(options.Search.ToLower())
                                     );

        if (options.HasSort())
            query = query.OrderBy($"{options.SortBy} {options.OrderType}");
        else
            query = query.OrderByDescending(a => a.Id).ThenByDescending(b => b.DocOn);

        return query;
    }
}
