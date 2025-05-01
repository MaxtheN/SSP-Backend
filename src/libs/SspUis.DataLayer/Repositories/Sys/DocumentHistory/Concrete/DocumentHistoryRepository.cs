using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentHistoryRepository : BaseEntityRepository<long, DocumentHistory>, IDocumentHistoryRepository
    {
        public DocumentHistoryRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public DocumentHistory Create(DocumentHistoryDlDto dto)
        {
            DocumentHistory entity = dto.CreateEntity();
            Context.Set<DocumentHistory>().Add(entity);
            Context.Entry(entity).State = EntityState.Added;

            if (entity == null)
                return null;

            Context.SaveChanges();

            return entity;
        }
    }
}
