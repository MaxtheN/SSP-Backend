using System.Linq;
using WEBASE.Models;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.Propos
{
    public static class ProposalListDtoSortFilter
    {
        public static IQueryable<ProposalListDto> SortFilter(this IQueryable<ProposalListDto> query, ProposalSortFilterOptions options)
        {
            if (options.ExternalSourctTypeId.HasValue)
                query = query.Where(a => a.ExternalSourceTypeId == options.ExternalSourctTypeId);
            if (options.FromDocDate.HasValue)
                query = query.Where(a => a.DocOn >= options.FromDocDate.Value);
            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.DocOn <= options.ToDocDate.Value);
            if (options.HasSearch())
                query = query.Where(a => a.Id.ToString().ToLower().Contains(options.Search.ToLower()));

            if (options.HasSearch())
                query = query.Where(a => a.DocNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.ProposalTypeName.ToLower().Contains(options.Search.ToLower())
                                      || a.BusinessSectorName.ToLower().Contains(options.Search.ToLower())
                                      || a.CompanyInn.ToLower().Contains(options.Search.ToLower())
                                      || a.ExternalSourceTypeName.ToLower().Contains(options.Search.ToLower())
                                      || a.NameLatin.ToLower().Contains(options.Search.ToLower())
                                      || a.NameEng.ToLower().Contains(options.Search.ToLower())
                                      || a.AddressName.ToLower().Contains(options.Search.ToLower())
                                      || a.SurnameEng.ToLower().Contains(options.Search.ToLower())
                                      || a.SurnameLatin.ToLower().Contains(options.Search.ToLower())
                                      || a.PatronymLatin.ToLower().Contains(options.Search.ToLower())
                                      || a.BirthDate.ToString().ToLower().Contains(options.Search.ToLower())
                                      || a.GenderName.ToLower().Contains(options.Search.ToLower())
                                      || a.PhoneNumber.ToLower().Contains(options.Search.ToLower())
                                      || a.Email.ToLower().Contains(options.Search.ToLower())
                                      || a.MfyName.ToLower().Contains(options.Search.ToLower())
                                      || a.EmployementTypeName.ToLower().Contains(options.Search.ToLower())
                                      || a.ProposalDisclosureName.ToLower().Contains(options.Search.ToLower())
                                      || a.ToOrganizationName.ToLower().Contains(options.Search.ToLower())
                                      || a.ProposalText.ToLower().Contains(options.Search.ToLower())
                                      || a.AppealText.ToLower().Contains(options.Search.ToLower())
                                      || a.RegionName.ToLower().Contains(options.Search.ToLower())
                                      || a.DistrictName.ToLower().Contains(options.Search.ToLower())
                                      || a.CompanyName.ToLower().Contains(options.Search.ToLower()));

            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);

            return query;
        }
    }
}
