using System.Threading.Tasks;
using Quartz;
using SspUis.BizLogicLayer.Hrm;

namespace SspUis.BizLogicLayer.IntegrationServices
{
    public class MehnatJobService : IJob
    {
        private readonly IMehnatService _service;

        public MehnatJobService(IMehnatService service,
            IEmployeeWorkScheduleService employeeWorkScheduleService,
            IEmployeeService employeeService)
        {
            _service = service;
        }
        public async Task Execute(IJobExecutionContext context)
        {

            _service.UpdateEmployeesWorkData();
        }
    }
}
