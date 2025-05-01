using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfCode;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Job.WebApi.Hangfire.JobServices
{
    public class CustomJobHangfireService : StatusGenericHandler, ICustomJobHangfireService
    {
        private readonly EfCoreContext _context;
        private readonly ICustomJobRunnerFactory _customJobRunnerFactory;

        public CustomJobHangfireService(
            EfCoreContext context, 
            ICustomJobRunnerFactory customJobRunnerFactory)
        {
            _context = context;
            _customJobRunnerFactory = customJobRunnerFactory;
        }

        public async Task RunAsync(CustomJobParameter parameters, CancellationToken cancellationToken)
        {
            var customJob = _context.CustomJobs
                .Where(a => a.Id == parameters.CustomJobId)
                //.Select(a => new { a.StatusId, a.JobTypeId })
                .FirstOrDefault();
            if (customJob == null)
                throw new Exception($"Custom job with id = {parameters.CustomJobId} not found");
            customJob.StatusId = StatusIdConst.EXECUTING;
            _context.SaveChanges();

            var customJobRunner = _customJobRunnerFactory.GetJobRunner(customJob.JobTypeId);
            await customJobRunner.Run(parameters.CustomJobId, cancellationToken);
            customJob.StatusId = StatusIdConst.APPROVED;
            _context.SaveChanges();


        }
    }
}
