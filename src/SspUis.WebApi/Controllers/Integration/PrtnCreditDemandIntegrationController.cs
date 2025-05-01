using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.PrtnCreditDemandServices;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("creditDemandIntegration/[action]")]
    public class PrtnCreditDemandIntegrationController :
        WebaseController
    {
        private IPrtnCreditDemandService _service;

        public PrtnCreditDemandIntegrationController(IPrtnCreditDemandService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
       
        [HttpGet]
        [BasicAuth("bank_kredit")]
        [ProducesResponseType(200)]
        public IActionResult GetCheckCreditDemandWithCertificate(Guid? Id2, string? contractorInn)
        {

            if (ModelState.IsValid)
            {
                var result = _service.GetCheckCreditDemandWithCertificate(Id2, contractorInn);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
