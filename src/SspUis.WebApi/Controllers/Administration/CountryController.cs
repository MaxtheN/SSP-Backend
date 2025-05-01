using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.CountryServices;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Soliq.Models;
using Humanizer;
using SspUis.BizLogicLayer.Administration.CountryServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("country/[action]")]
    public class CountryController : WebaseController
    {
        private ICountryService _service;

        public CountryController(ICountryService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.CountryView)]
        public PagedResult<CountryListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.CountryView)]
        [ProducesResponseType(typeof(CountryDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.CountryView)]
        [ProducesResponseType(typeof(CountryDto), 200)]
        public IActionResult Get(int id)
        {
            CountryDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<int>), 200)]

        public async Task<ActionResult> GetAsSelectListForBojXona(AsSelectListForBojxonaDto dto)
        {
            var result = await _service.AsSelectListForBojxona(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CountryCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateCountryDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<int> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CountryEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateCountryDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [Authorize(ModuleCode.CountryDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _service.Delete(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
