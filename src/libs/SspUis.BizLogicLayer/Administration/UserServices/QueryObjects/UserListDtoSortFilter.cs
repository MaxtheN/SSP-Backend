using System.Linq;
using System.Linq.Dynamic.Core;

namespace SspUis.BizLogicLayer.UserServices
{
    public static class UserListDtoSortFilter
    {
        public static IQueryable<UserListDto> SortFilter(this IQueryable<UserListDto> query, UserSortFilterPageOptions options)
        {
            // 1 = Active, 2 = passive
            query = query.Where(x => x.StateId == 1);

            if (options.HasSearch())
                query = query.Where(a => a.FullName.ToLower().Contains(options.Search.ToLower())
                    || a.UserName.ToLower().Contains(options.Search.ToLower())
                    || a.PhoneNumber.ToLower().Contains(options.Search.ToLower())
                    || a.Organization.ToLower().Contains(options.Search.ToLower())
                    || a.OrganizationInn.ToLower().Contains(options.Search.ToLower())
                );

            if (options.RoleId.HasValue && options.RoleId.Value != 0)
                query = query.Where(a => a.UserRoles.Any(x => x == options.RoleId.Value));

            if (options.ModuleId.HasValue && options.ModuleId.Value > 0)
                query = query.Where(a => a.UserModels.Any(x => x == options.ModuleId.Value));

            query.ToList();
            if (options.HasSort())
                query = query.OrderBy($"{options.SortBy} {options.OrderType}");
            else
                query = query.OrderByDescending(a => a.Id);
            return query;
        }

        public static IQueryable<UserListDto> FilterByCustomFields(this IQueryable<UserListDto> query, UserSortFilterPageOptions options)
        {
            if (options.OrganizationId.HasValue)
                query = query.Where(a => a.OrganizationId == options.OrganizationId);

            return query;
        }
    }
}
