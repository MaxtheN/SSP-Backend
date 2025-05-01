using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Administration.CustomJobServices.JobRunners
{
    public interface ICustomJobRunner
    {
        Task Run(long jobId, CancellationToken cancellationToken, string? backgroundJobId = null);
    }

}
