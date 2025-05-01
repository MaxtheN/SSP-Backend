using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IDocumentChatRepository : IBaseEntityRepository<long, DocumentChat, CreateDocumentChatDlDto, UpdateDocumentChatDlDto>
    {

    }
}
