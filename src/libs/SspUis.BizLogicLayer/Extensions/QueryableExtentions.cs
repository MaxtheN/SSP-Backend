using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using WEBASE;

namespace SspUis.BizLogicLayer
{
    public static class QueryableExtentions
    {
        public static WEBASE.Models.PagedResult<T> ToTableData<T>(this IQueryable<T> queryable, TableSortFilterPageOptions options)
        {
            return queryable.Filter(options.Filters).AsPagedResult(options);
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IDictionary<string, FilterMeta> filters)
        {
            if (filters == null) return queryable;
            filters = filters.Where(a => a.Value != null && a.Value.Value != null).ToDictionary(a => a.Key, a => a.Value);
            if (filters != null && filters.Any())
            {
                var predicateItems = new List<string>();
                int i = 0;
                foreach (var filter in filters)
                {
                    predicateItems.Add(filter.ToExpression(i++));
                }
                var predicate = string.Join(" AND ", predicateItems);
                queryable = queryable.Where($"({predicate})", filters.Select(f => f.Value.Value).ToArray());
            }
            return queryable;
        }


        private static string ToExpression(this KeyValuePair<string, FilterMeta> keyValuePair, int index)
        {
            string comparison = keyValuePair.Value.MatchMode.ToLower();

            if (keyValuePair.Value.MatchMode == "eq" || keyValuePair.Value.MatchMode == "equal" || keyValuePair.Value.MatchMode == "equals")
                return $"{keyValuePair.Key}=@{index}";
            
            if (keyValuePair.Value.MatchMode == "neq" || keyValuePair.Value.MatchMode == "notequal" || keyValuePair.Value.MatchMode == "notequals")
                return $"{keyValuePair.Key}!=@{index}";

            if (keyValuePair.Value.MatchMode == "doesnotcontain")
                return $"!{keyValuePair.Key}.{comparison}(@{index})";

            if (comparison == "startswith" || comparison == "endswith" || comparison == "contains")
            {
                keyValuePair.Value.Value = ((string)keyValuePair.Value.Value).ToLower();
                return $"{keyValuePair.Key}.ToLower().{comparison}(@{index})";
            }
            return $"{keyValuePair.Key} {comparison} @{index}";
        }
    }
}
