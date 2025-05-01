using System.Linq;

namespace SspUis.BizLogicLayer
{
    public static class DocumentModelSortFilter
    {
        public static IQueryable<TDocument> DocumentFilter<TDocument, TSort>(this IQueryable<TDocument> query, TSort options)
            where TDocument : IDocumentListModel
            where TSort : DocumentSortFilterOptions
        {
            if (options.FromDocDate.HasValue)
                query = query.Where(a => options.FromDocDate.Value <= a.DocOn);

            if (options.ToDocDate.HasValue)
                query = query.Where(a => a.DocOn <= options.ToDocDate.Value);

            if (options.StatusIds != null && options.StatusIds.Any())
                query = query.Where(a => options.StatusIds.Any(x => x == a.StatusId));

            return query;
        }
    }
}