using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Arbitration;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers.Arbitration.Dashboard
{
    [Route("[controller]/")]
    [Authorize]
    [ApiController]
    public class ArbitrationApplicationDashboardController : WebaseController
    {
        private readonly IArbitrationDashboardService _service;
        public ArbitrationApplicationDashboardController(IArbitrationDashboardService service)
        {
            _service = service;
        }
        [HttpPost("GetArbitrationTypeCount")]
        [Authorize(ModuleCode.ArbitrationDashboardView)]
        [ProducesResponseType(typeof(ArbitrationTypeCountDto), 200)]
        public IActionResult GetArbitrationTypeCount(ArbitrationDashFilterOptions options)
        {
            var res = _service.GetAllArbitrationTypeAmount(options);
            return Ok(res);
        }

        [HttpPost("GetArbitrationRate")]
        [Authorize(ModuleCode.ArbitrationDashboardView)]
        [ProducesResponseType(typeof(ArbitrationRateDto), 200)]
        public IActionResult GetArbitrationRate(ArbitrationRateFilterOptions options)
        {
            var res = _service.GetAllArbitrationRateDto(options);
            return Ok(res);
        }

        [HttpPost("GetAllDeadlineNearApplication")]
        [Authorize(ModuleCode.ArbitrationDashboardView)]
        [ProducesResponseType(typeof(DeadlineNearArbitrationApplicationDto), 200)]
        public IActionResult GetAllDeadlineNearApplication(ArbitrationApplicationDtoFilter options)
        {
            var res = _service.GetAllDeadlineNearApplication(options);
            return Ok(res);
        }
    }
}