using Quartz;
using SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
using SspUis.Integration.Finance.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.IntegrationServices;

public class FinanceIntegrationJobService : IJob
{
    private readonly IFinanceIntegrationService _service;
    private readonly IFinanceService _financeService;

    private List<string> codes = new List<string> 
    {
      "20212000003781497001",
      "20212000803781497025",
      "20212000503781497051",
      "20212000803781497026",
      "20212000903781497007",
      "20212000803781497022"
    };

    public FinanceIntegrationJobService(IFinanceIntegrationService service, IFinanceService financeService)
    {
        _service = service;
        _financeService = financeService;
    }
    public async Task Execute(IJobExecutionContext context)
    { 
        
        var startDate = new DateOnly(2023, 10, 1);
        var endDate = new DateOnly(2024, 5, 29);
        var count = endDate.DayNumber - startDate.DayNumber;

        for(int i = 0; i <= count; i++) 
        {
            foreach (var code in codes)
            {
                var date = startDate.AddDays(i).ToString("ddMMyyyy");
                var data = await _financeService.GetPayDocsAsync(code, date);
                _service.SaveFinancePayDocsByAcc(data);
            }

        }
       
    }
}
