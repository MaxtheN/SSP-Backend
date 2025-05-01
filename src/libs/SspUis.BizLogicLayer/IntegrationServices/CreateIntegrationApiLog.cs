using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.IntegrationServices;

public class CreateIntegrationApiLog : IJob
{
    private readonly IIntegrationService _integrationService;
    public CreateIntegrationApiLog(IIntegrationService integrationService)
    {
        _integrationService = integrationService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        await _integrationService.CheckAllIntegrations();
    }
}
