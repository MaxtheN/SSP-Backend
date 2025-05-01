using Microsoft.AspNetCore.Mvc;
using Quartz.Util;
using SspUis.BizLogicLayer.IntegrationServices.Soliq;
using SspUis.Core.Security;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.My.WebApi.Controllers.Integration
{
    [ApiController]
    [Route("Soliq/[action]")]
    public class SoliqIntegrationController : WebaseController
    {
        private readonly ISoliqContractorService _service;
        private readonly ISoliqService _soliqService;
        private readonly IAuthService _authService;
        public SoliqIntegrationController(ISoliqContractorService service, ISoliqService soliqService, IAuthService authService)
        {
            _service = service;
            _soliqService = soliqService;
            _authService = authService;
        }

        [HttpGet("{pinfl}")]
        [ProducesResponseType(typeof(SoliqContractorDebtByPinflDto), 200)]
        public async Task<ActionResult> GetByPinfl(string pinfl)
        {
            var dto = await _service.GetByPinfl(pinfl);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CompanyInfo), 200)]
        public async Task<ActionResult> GetCompanyCriteries()
        {
            var data = _authService.Contractor.Inn;
            if(data.IsNullOrWhiteSpace())
                return NotFound();

            var dto = await _soliqService.GetCompanyCriteries(data);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
