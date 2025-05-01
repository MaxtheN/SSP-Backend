using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.Integration.AgroBank.Services;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers.Integration
{
    [Route("agroBankIntegration/[action]")]
    [ApiController]
    public class AgroBankIntegrationController : WebaseController
    {
        private readonly IAgroBankService _service;
        public AgroBankIntegrationController(IAgroBankService service) : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetLoanActualData(string loanId)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetLoanActualData(loanId);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

    }
}
