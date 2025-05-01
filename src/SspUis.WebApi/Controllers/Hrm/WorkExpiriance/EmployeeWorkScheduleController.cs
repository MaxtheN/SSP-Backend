using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("EmployeeWorkSchedule/[action]")]
    public class EmployeeWorkScheduleController : WebaseController
    {
        private readonly IEmployeeWorkScheduleService _service;
        public EmployeeWorkScheduleController(IEmployeeWorkScheduleService service)
            => _service = service;
        [HttpPost]
        //[Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetWorkYearFromEmpManage(EmployeeWorkScheduleFromSSPFilterOption option)
        {
            return Ok(_service.GetWorkYearFromEmpManage(option));
        }
        [HttpPost]
        //[Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetTotalWorkYears(EmployeeWorkScheduleFilterOption option)
        {
            return Ok(_service.GetTotalWorkYears(option));
        }
        [HttpPost]
        //[Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetWorkYearFromMehnat(EmployeeWorkScheduleFromMehnatFilterOption option)
        {
            return Ok(_service.GetWorkYearFromMehnat(option));
        }
    }
}