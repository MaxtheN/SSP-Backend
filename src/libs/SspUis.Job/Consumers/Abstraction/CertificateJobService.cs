using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.RabbitMQ.Messages;
using StatusGeneric;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.RabbitMQ.Abstractions
{
    public abstract class CertificateJobService<TEntity, TPublisher, TMessage> : StatusGenericHandler
        where TEntity : class, IJobDocumentEntity
        where TPublisher : PublisherMultipleBase<TMessage>
        where TMessage : DocumentMessage
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DbContext _dbContext;
        protected readonly IBaseEntityRepository<long, TEntity> _repository;
        private readonly TPublisher _publisher;
        private readonly IDocumentJobHistoryService _jobHistoryService;
        private readonly IAuthService _authService;

        public CertificateJobService(
            IHttpContextAccessor httpContextAccessor,
            DbContext dbContext,
            IBaseEntityRepository<long, TEntity> repository,
            TPublisher publisher,
            IDocumentJobHistoryService jobHistoryService,
            IAuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _repository = repository;
            _publisher = publisher;
            _jobHistoryService = jobHistoryService;
            _authService = authService;
        }

        protected IQueryable<DocumentJobHistory> GetNotFinishedJobs(long? docId, params int[] tableIds)
        {
            return _jobHistoryService.GetNotFinishedJobs(_authService.Contractor != null ? null : _authService.Organization.Id, docId, tableIds);
        }

        protected HaveId<long> Run(long id, int toStatusId)
        {
            var message = Activator.CreateInstance<TMessage>();
            var entity = _repository.ById(id);
            _dbContext.Set<PrtnCertificate>().FirstOrDefault(a => a.Id == id).TotalPostCount = _publisher.QueueList.Count;
            _dbContext.SaveChanges();

            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            return Run(message, entity, toStatusId);
        }

        protected HaveId<long> Run(TMessage message, int toStatusId)
        {
            var entity = _repository.ById(message.DocId);

            CombineStatuses(_repository);
            if (HasErrors)
                return null;

            return Run(message, entity, toStatusId);
        }

        private HaveId<long> Run(TMessage message, TEntity entity, int toStatusId)
        {
            if (entity is SspUis.DataLayer.EfClasses.Application)
            {
                if (!StatusIdConst.CanApplicationApplyStatus(entity.StatusId, toStatusId))
                {
                    AddError("Imkoni yo'q / Нет доступа");
                    return null;
                }
            }
            else
            if (entity is SspUis.DataLayer.EfClasses.PrtnCertificate)
            {
            }
            else
            {
                if (!StatusIdConst.CanApplyStatus(entity.StatusId, toStatusId))
                {
                    AddError("Imkoni yo'q / Нет доступа");
                    return null;
                }
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                entity.Message = null;
                //certificateni statusini ozgartirmaymiz.
                //entity.PrevStatusId = entity.StatusId;
                //entity.StatusId = StatusIdConst.WAITING;
                _dbContext.Entry(entity).State = EntityState.Modified;

                _dbContext.SaveChanges();

                if (IsValid)
                {
                    message.UserName = _authService.UserName;
                    message.OrganizationId = _authService.Contractor != null ? null : _authService.Organization.Id;
                    message.DocId = entity.Id;
                    message.TableId = entity.TableId;
                    message.FromStatusId = entity.PrevStatusId;
                    message.ToStatusId = toStatusId;
                    message.RequestTraceId = _httpContextAccessor?.HttpContext?.TraceIdentifier;
                    message.UserIp = _authService.UserIp;
                    message.UserAgent = _authService.UserAgent;

                    var jobHistory = CreateJobHistory(message);

                    if (HasErrors)
                        return null;

                    message.JobHistoryId = jobHistory.Id;

                    _publisher.Publish(message);
                }

                CombineStatuses(_publisher);

                if (IsValid)
                    transaction.Commit();
            }

            return HaveId.Create(entity.Id);
        }

        private DocumentJobHistory CreateJobHistory(TMessage message)
        {
            var documentJobHistory = _jobHistoryService.Create(new CreateDocumentJobHistoryDlDto
            {
                DocId = message.DocId,
                TableId = message.TableId,
                UserId = (int)_authService.UserId,
                FromStatusId = message.FromStatusId ?? StatusIdConst.CREATED,
                ToStatusId = message.ToStatusId,
                OrganizationId = _authService.Contractor != null ? null : _authService.Organization.Id,
                RequestTraceId = message.RequestTraceId
            });

            CombineStatuses(_jobHistoryService);
            if (HasErrors)
                return null;

            return documentJobHistory;
        }
    }
}
