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
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.Hrm.SpecialtyServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("dualedu/[controller]/[action]")]
    public class SpecialtyController : WebaseController
    {
        private ISpecialtyService _service;

        public SpecialtyController(ISpecialtyService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.SpecialtyView)]
        public PagedResult<SpecialtyListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.SpecialtyView)]
        [ProducesResponseType(typeof(SpecialtyDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SpecialtyView)]
        [ProducesResponseType(typeof(SpecialtyDto), 200)]
        public IActionResult Get(int id)
        {
            SpecialtyDto dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            
            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList(int? instituteId = null)
        {
            return Ok(_service.AsSelectList(instituteId));
        }

        [HttpPost]
        [Authorize(ModuleCode.SpecialtyCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateSpecialtyDlDto dto)
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
        [Authorize(ModuleCode.SpecialtyEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateSpecialtyDlDto dto)
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
        [Authorize(ModuleCode.SpecialtyDelete)]
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
