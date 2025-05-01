using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.DocumentHistoryServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.RabbitMQ.Abstractions;
using SspUis.RabbitMQ.Certificate.Messages;
using SspUis.RabbitMQ.Certificate.Publishers;

namespace SspUis.Job.BizLogicLayer.Services
{
    public interface IPrtnCertificateJobService
    {
        void RequestToSent(long id);
    }

    public class PrtnCertificateJobService : CertificateJobService<PrtnCertificate, PrtnCertificatePublisher, PrtnCertificateMessageBase>, IPrtnCertificateJobService
    {
        public PrtnCertificateJobService(
        IHttpContextAccessor httpContextAccessor,
            DbContext dbContext,
            IPrtnCertificateRepository repository,
            PrtnCertificatePublisher publisher,
            IDocumentJobHistoryService documentJobHistoryService,
            IAuthService authService)
            : base(httpContextAccessor, dbContext, repository, publisher, documentJobHistoryService, authService)
        {
        }


        public void RequestToSent(long id)
        {
            var notFinishedJobs = GetNotFinishedJobs(id, new int[] { TableIdConst.DOC_PRTN_CERTIFICATE/* can be more related document tableIds here */ });
            if (notFinishedJobs.Any())
            {
                AddError("Есть незавершенное выполнение документов. Попробуй позже.");
                return;
            }

            Run(id, StatusIdConst.FORMED);
        }
    }
}
