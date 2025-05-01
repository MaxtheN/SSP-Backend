using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public interface IDocumentJobHistoryService : IStatusGeneric
    {
        IQueryable<DocumentJobHistory> GetNotFinishedJobs(int? organizationId, long? docId, params int[] tableIds);
        DocumentJobHistory ById(long id);
        DocumentJobHistory Create(CreateDocumentJobHistoryDlDto dto);
        DocumentJobHistory Start(long id);
        HaveId<long> End(long id, bool succeed, string errorMessage = null);
        List<DocumentJobHistory> GetByTableId(long tableId, long docId);
    }
}
