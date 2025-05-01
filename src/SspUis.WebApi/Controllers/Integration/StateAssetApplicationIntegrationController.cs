using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Core.Security;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers.Integration
{
    [ApiController]
    [Route("stateassetapplicationintegration/[action]")]
    public class StateAssetApplicationIntegrationController : Controller
    {
        private IAuthService _authService;
        private IStateAssetApplicationService _service;

        public StateAssetApplicationIntegrationController(
            IStateAssetApplicationService service,
            IAuthService authService)
        {
            _service = service;
            _authService = authService;
            _authService = authService;
        }

        [HttpPost]
        [BasicAuth("davaktiv")]
        [ProducesResponseType(200)]
        public IActionResult UpdateStatus(UpdateStateAssetStatusStateAssetApplicationDto dto)
        {
            _authService.ResetUserName("davakiv");

            if (ModelState.IsValid)
            {
                var result = _service.UpdateStateAssetStatus(dto);

                return Ok(result);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet("id")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendToDavAktiv(long id)
        {

            if (ModelState.IsValid)
            {
                await _service.SendToDavAktiv(id);

                return Ok();
            }

            return ValidationProblem(ModelState);
        }
    }
}
