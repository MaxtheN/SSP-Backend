using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IDocumentJobHistoryRepository : IBaseEntityRepository<long, DocumentJobHistory, CreateDocumentJobHistoryDlDto, UpdateDocumentJobHistoryDlDto>
    {
        DocumentJobHistory End(long id, bool isSucceed, string message = null);
    }
}
