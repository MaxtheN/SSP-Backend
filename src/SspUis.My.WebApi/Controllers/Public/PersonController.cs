using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.Integration.DigitizationCenter.Models.GSP;
using WEBASE.AspNet;
using WEBASE.Integration.MSPD.GSP;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PersonController : WebaseController
    {
        private IPersonService _service;
        public PersonController(IPersonService service)
            : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public async Task<IActionResult> GetByPassportData([FromQuery] GSPPersonInfoRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetByPassportData(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PersonDto), 200)]
        public async Task<IActionResult> GetByPassportDataFromDigital([FromQuery] GSPNewApiRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var person = await _service.GetByPassportDataFromDigital(dto);

                if (_service.IsValid)
                    return Ok(person);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
