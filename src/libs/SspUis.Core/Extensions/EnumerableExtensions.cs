using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SafeUnion<T>(this IEnumerable<T> source1, IEnumerable<T> source2)
        {
            return source1 != null
                    ? (source2 != null ? source1.Union(source2) : source1)
                    : source2;
        }
    }
}
