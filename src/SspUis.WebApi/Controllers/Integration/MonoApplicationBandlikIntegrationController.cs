using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.IntegrationServices;
using WEBASE.AspNet.Security;
using WEBASE.AspNet;
using SspUis.DataLayer.Repositories;

namespace SspUis.WebApi.Controllers.Integration
{
    [Route("monoAppBandlikIntegration/[controller]")]
    [ApiController]
    public class MonoApplicationBandlikIntegrationController : WebaseController
    {
        private readonly IMonoApplicationIntegrationService _service;

        public MonoApplicationBandlikIntegrationController(IMonoApplicationIntegrationService service)
        {
            _service = service;
        }

        [BasicAuth("mono_application_bandlik")]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult PostMonoApplicationBandlik(MonoApplicationBandlikResultDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.CreateMonoApplicationBandlik(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}