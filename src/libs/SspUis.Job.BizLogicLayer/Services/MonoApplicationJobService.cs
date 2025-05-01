using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Application.Messages;
using SspUis.RabbitMQ.Application.Publishers;
using StatusGeneric;

namespace SspUis.Job.BizLogicLayer.Services
{
    public interface IMonoApplicationJobService : IStatusGeneric
    {
        void RequestToSentForReview(long id);
    }

    public class MonoApplicationJobService : DocumentJobService<MonoApplication, MonoApplicationPublisher, MonoApplicationMessage>, IMonoApplicationJobService
    {
        public MonoApplicationJobService(
            IHttpContextAccessor httpContextAccessor,
            DbContext dbContext,
            IMonoApplicationRepository repository,
            MonoApplicationPublisher publisher,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService)
            : base(httpContextAccessor, dbContext, repository, publisher, documentJobHistoryService, authService)
        {
        }


        public void RequestToSentForReview(long id)
        {
            var notFinishedJobs = GetNotFinishedJobs(id, new int[] { TableIdConst.DOC_MONO_APPLICATION /* can be more related document tableIds here */ });
            if (notFinishedJobs.Any())
            {
                AddError("Есть незавершенное выполнение документов. Попробуй позже.");
                return;
            }

            Run(id, StatusIdConst.SENT_FOR_REVIEW);
        }

    }
}
