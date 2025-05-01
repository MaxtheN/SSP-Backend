using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public interface IDocumentChatService : IStatusGeneric
    {
        PagedResult<DocumentChatListDto> GetList(DocumentChatListDtoSortFilterPageOption dto);
        HaveId<long> Create(CreateDocumentChatDlDto dto);
        void Delete(long id);
    }
}
