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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Job.BizLogicLayer.Services
{
    public interface IApplicationJobService: IStatusGeneric
    {
        void RequestToSentForReview(long id);
    }

    public class ApplicationJobService : DocumentJobService<Application, PrtnApplicationPublisher, PrtnApplicationMessage>, IApplicationJobService
    {
        public ApplicationJobService(
            IHttpContextAccessor httpContextAccessor,
            DbContext dbContext,
            IApplicationRepository repository,
            PrtnApplicationPublisher publisher,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService)
            : base(httpContextAccessor, dbContext, repository, publisher, documentJobHistoryService, authService)
        {
        }


        public void RequestToSentForReview(long id)
        {
            var notFinishedJobs = GetNotFinishedJobs(id, new int[] { TableIdConst.DOC_APPLICATION /* can be more related document tableIds here */ });
            if (notFinishedJobs.Any())
            {
                AddError("Есть незавершенное выполнение документов. Попробуй позже.");
                return;
            }

            Run(id, StatusIdConst.SENT_FOR_REVIEW);
        }

    }
}
