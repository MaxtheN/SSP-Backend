using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace SspUis.BizLogicLayer.IntegrationServices.Xodim
{
    public class WorkActivityJobService : IJob
    {
        private readonly IWorkActivity _service;

        public WorkActivityJobService(IWorkActivity service)
        {
            _service = service;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _service.UpdateWorkActivity();
        }
    }
}
