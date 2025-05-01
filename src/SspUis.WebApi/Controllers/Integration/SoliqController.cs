using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.IntegrationServices.Soliq;
using SspUis.Core.Security;
using SspUis.Integration.Soliq.Models;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers.Integration
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SoliqController : WebaseController
    {
        private readonly ISoliqService _soliqService;


        public SoliqController(ISoliqService soliqService)
        {
            _soliqService = soliqService;
        }

        [HttpGet("{tin}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyInfo), 200)]
        public async Task<ActionResult> GetCompanyCriteries(string tin)
        {
            var dto = await _soliqService.GetCompanyCriteries(tin);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyCriteriesData), 200)]
        public async Task<ActionResult> GetCompanyHighNewInfo([FromQuery] int isBusiness, [FromQuery] int ns10Code, [FromQuery] int ns11Code)
        {
            var dto = await _soliqService.GetCompanyHighNewInfo(isBusiness, ns10Code, ns11Code);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyStateNewInfoData), 200)]
        public async Task<ActionResult> GetCompanyStateNewInfo([FromQuery] int isBusiness, [FromQuery] int ns10Code, [FromQuery] int ns11Code)
        {
            var dto = await _soliqService.GetCompanyStateNewInfoData(isBusiness, ns10Code, ns11Code);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyStateNewInfoData), 200)]
        public async Task<ActionResult> GetQqsAylanma([FromQuery] int month, [FromQuery] int inn, [FromQuery] int year)
        {
            var dto = await _soliqService.GetQqsAylanmaData(month, inn, year);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyStateNewInfoData), 200)]
        public async Task<ActionResult> GetAosAylanma([FromQuery] int inn, [FromQuery] int year)
        {
            var dto = await _soliqService.GetAosAylanmaData(year: year, inn: inn);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CompanyStateNewInfoData), 200)]
        public async Task<ActionResult> GetSoliqImtiyozlari([FromQuery] decimal STIR, [FromQuery] int year)
        {
            var dto = await _soliqService.GetSoliqImtiyozlari(STIR, year);

            if (_soliqService.IsValid)
                return Ok(dto);

            _soliqService.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }

}
