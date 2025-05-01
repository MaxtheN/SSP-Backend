using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.IntegrationServices.Bojxona;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Bojxona.Services;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Integration
{
    [ApiController]
    [Route("bojxonaIntegration/[action]")]
    public class BojxonaIntegrationController: WebaseController
    {
        private readonly IBojxonaIntegrationService _service;

        public BojxonaIntegrationController(IBojxonaIntegrationService service)
        {
            _service = service;
        }

        [BasicAuth("bojxona_imtiyoz")]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult PostBojxonaImtiyoz(BojxonaImtiyozDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.CreateBojxonaImtiyoz(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
