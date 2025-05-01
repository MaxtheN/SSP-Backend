using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("MemshipDashboard/[action]")]
    public class MemshipDashboardController : WebaseController
    {
        private readonly IMemshipDashboardService _service;
        public MemshipDashboardController(IMemshipDashboardService service)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipStatistic(MemshipDashFilterOption options)
        {
            return Ok(_service.GetMemshipStatisticList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipApplicationList(MemshipDashFilterOption options)
        {
            return Ok(_service.GetMemshipApplicationList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipContractList(MemshipDashFilterOption options)
        {
            return Ok(_service.GetMemshipContractList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipCertificateList(MemshipDashFilterOption options)
        {
            return Ok(_service.GetMemshipCertificateList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipContractTypeList(MemshipDashFilterOption options)
        {
            return Ok(_service.GetMemshipContractTypeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipDashboardView)]
        public IActionResult GetMemshipContractRateList(MemshipDashRateFilterOption options)
        {
            return Ok(_service.GetMemshipContractRateList(options));
        }
    }
}
