using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.CustomJob.Publishers;
using SspUis.RabbitMQ.CustomJob.Messages;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Job.BizLogicLayer.Services
{
    public interface ICustomJobService : IStatusGeneric
    {
        void RequestToSent(long id);
    }

    public class CustomJobService : DocumentJobService<CustomJob, CustomJobPublisher, CustomJobMessage>, ICustomJobService
    {
        public CustomJobService(
            IHttpContextAccessor httpContextAccessor,
            DbContext dbContext,
            ICustomJobRepository repository,
            CustomJobPublisher publisher,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService)
            : base(httpContextAccessor, dbContext, repository, publisher, documentJobHistoryService, authService)
        {
        }

        public void RequestToSent(long id)
        {
            var notFinishedJobs = GetNotFinishedJobs(id, new int[] { TableIdConst.SYS_CUSTOM_JOB /* can be more related document tableIds here */ });
            if (notFinishedJobs.Any())
            {
                AddError("Есть незавершенное выполнение документов. Попробуй позже.");
                return;
            }

            Run(id, StatusIdConst.APPROVED);
        }

    }
}
