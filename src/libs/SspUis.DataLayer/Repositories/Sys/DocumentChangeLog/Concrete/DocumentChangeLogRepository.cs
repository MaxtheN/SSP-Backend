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
    public class DocumentChangeLogRepository : BaseEntityRepository<long, DocumentChangeLog, CreateDocumentChangeLogDlDto, UpdateDocumentChangeLogDlDto>, IDocumentChangeLogRepository
    {
        public DocumentChangeLogRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

    }
}
