using System.Linq;
namespace SspUis.BizLogicLayer;
public static class DocumentChatListDtoSortFilter
{
    public static IQueryable<DocumentChatListDto> SortFilter(
        this IQueryable<DocumentChatListDto> query,
        DocumentChatListDtoSortFilterPageOption options)
    {
        query = options.HasSearch()
            ? query.Where(a => a.MessageText.ToLower().Contains(options.Search.ToLower()))
            : query.OrderByDescending(a => a.Id);

        return query.Where(a => a.TableId == options.TableId && a.DocumentId == options.DocumentId);
    }
}