using WEBASE.Models;
using StatusGeneric;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using System.Linq;
using SspUis.Core;
using System;
using WEBASE;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public class DocumentJobHistoryService : StatusGenericHandler, IDocumentJobHistoryService
    {
        private readonly IDocumentJobHistoryRepository _repository;

        public DocumentJobHistoryService(IDocumentJobHistoryRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<DocumentJobHistory> GetNotFinishedJobs(int? organizationId, long? docId, params int[] tableIds)
        {
            var query = _repository.AllAsQueryable
                                   .Where(a => a.StatusId != StatusIdConst.FINISHED &&
                                               a.StatusId != StatusIdConst.FAILED &&
                                               (!docId.HasValue || a.DocId == docId.Value) &&
                                               (!organizationId.HasValue || a.OrganizationId == organizationId.Value));

            if (tableIds != null && tableIds.Length > 0)
                query = query.Where(a => tableIds.Contains(a.TableId));

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            return query;
        }

        public DocumentJobHistory ById(long id)
        {
            var entity = _repository.ById(id);

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            return entity;
        }

        public DocumentJobHistory Create(CreateDocumentJobHistoryDlDto dto)
        {
            var entity = _repository.Create(dto);

            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            entity.StatusId = StatusIdConst.WAITING;
            _repository.Context.SaveChanges();

            return entity;
        }

        public DocumentJobHistory Start(long id)
        {
            var entity = _repository.ById(id);

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            entity.StartAt = DateTime.Now;
            entity.StatusId = StatusIdConst.EXECUTING;
            _repository.Context.SaveChanges();

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            return entity;
        }

        public HaveId<long> End(long id, bool succeed, string errorMessage = null)
        {
            var entity = _repository.End(id, succeed, errorMessage.SafeSubstring(800));

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            _repository.Context.SaveChanges();

            return HaveId.Create(entity.Id);
        }

        public List<DocumentJobHistory> GetByTableId(long tableId, long docId)
        {
            var entities = _repository.AllAsQueryable.Where(a => a.TableId == tableId && a.DocId == docId).OrderByDescending(a => a.StartAt).ToList();

            CombineStatuses(_repository);

            if (HasErrors)
                return null;

            return entities;
        }
    }
}
