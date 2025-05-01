using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public class DocumentChangeLogDto : UpdateDocumentChangeLogDlDto, ILinkToEntity<DocumentChangeLog>
    {
        public string Table { get; set; }
        public string Status { get; set; }
    }
}
