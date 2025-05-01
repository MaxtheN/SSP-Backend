using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentChangeLogServices
{
    public interface IDocumentChangeLogService : IStatusGeneric
    {
        HaveId<long> Create<TId, TDocumentDto>(TDocumentDto dto, int tableId, int? organizationId, int statusId, string message = null, string userIp = null, string userAgent = null)
            where TDocumentDto : class, IDocument<TId>;
        HaveId<long> Create<TDocumentDto>(TDocumentDto dto, int tableId, int? organizationId, int statusId, string message = null, string userIp = null, string userAgent = null)
            where TDocumentDto : class, IDocument;
        HaveId<long> CreateApplication<TApplicationDto>(TApplicationDto dto, int? organizationId, string message = null)
         where TApplicationDto : class, IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>, new();
        List<DocumentChangeLogListDto> GetListByDocumentId(int tableId, long docId);
    }
}
