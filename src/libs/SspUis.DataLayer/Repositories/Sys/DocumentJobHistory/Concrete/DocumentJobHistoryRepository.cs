using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentJobHistoryRepository : BaseEntityRepository<long, DocumentJobHistory, CreateDocumentJobHistoryDlDto, UpdateDocumentJobHistoryDlDto>, IDocumentJobHistoryRepository
    {
        public DocumentJobHistoryRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<DocumentJobHistory> ByIdQuery()
            => AllAsQueryable;

        public DocumentJobHistory End(long id, bool isSucceed, string message = null)
        {
            var entity = ById(id);

            entity.IsSucceed = isSucceed;
            entity.EndAt = DateTime.Now;
            entity.StatusId = isSucceed ? StatusIdConst.FINISHED : StatusIdConst.FAILED;

            if (!string.IsNullOrEmpty(message))
                entity.Message = message.Length > 800 ? message.Substring(0, 800) : message;

            Context.Entry(entity).State = EntityState.Modified;

            return entity;
        }
    }
}
