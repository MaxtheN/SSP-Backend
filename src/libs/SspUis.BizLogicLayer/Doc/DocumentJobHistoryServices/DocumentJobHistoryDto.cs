using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public class DocumentJobHistoryDto : UpdateDocumentJobHistoryDlDto, ILinkToEntity<DocumentJobHistory>
    {
        public string Table { get; set; }
        public string FromStatus { get; set; }
        public string ToStatus { get; set; }
        public string Status { get; set; }
    }
}
