using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using SspUis.BizLogicLayer.MfyServices;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("ExternalMfy/[action]")]
    public class MfyIntegrationController :
        WebaseController
    {
        private IMfyService _service;

        public MfyIntegrationController(IMfyService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        /*[BasicAuth("mfy")]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateMfys(List<CreateMfyExternalDto> dto)
        {
            if (ModelState.IsValid)
            {
                _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }*/
        /*
                [HttpGet]
                [ProducesResponseType(200)]
                public async Task<IActionResult> GetExternalInos()
                     => Ok(await _service.GetExternalList());
                */
    }
}
