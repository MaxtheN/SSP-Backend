using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer
{
    public class GroupedModel<TKey, TRow>
    {
        public TKey Key { get; set; }
        public IEnumerable<TRow> Rows { get; set; }
    }

    public static class GroupedModelExtensions
    {
        public static IEnumerable<GroupedModel<TKey, TRow>> AsGroupedModel<TKey, TRow>(this IEnumerable<IGrouping<TKey, TRow>> source)
        {
            return source.Select(a => new GroupedModel<TKey, TRow>
            {
                Key = a.Key,
                Rows = a
            });
        }
    }
}
