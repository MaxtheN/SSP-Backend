using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Integration.Bandlik.Models;
using WEBASE.AspNet;
using WEBASE.Integration.MSPD.GSP;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReportController : WebaseController
    {
        private IReportService _service;
        public ReportController(IReportService service)
            : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpPost]
        [ProducesResponseType(typeof(List<GetFreeAreaByInnDataDto>), 200)]

        public async Task<IActionResult> GetFreeAreaFromBandlik()
        {
            var result = await _service.GetFreeAreaFromBandlik();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
