using StatusGeneric;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public interface IDocumentHistoryService : IStatusGeneric
    {
        DocumentHistoryCompareDto CompareContents(int tableId, long previouesId, long currentId);

        DocLastMessageResponseDto GetLastMessage(DocLastMessageRequestDto dto);
    }
}
