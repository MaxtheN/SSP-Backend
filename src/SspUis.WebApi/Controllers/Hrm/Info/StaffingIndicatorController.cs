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
using SspUis.BizLogicLayer.Hrm.StaffingIndicatorServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class StaffingIndicatorController : WebaseController
    {
        private IStaffingIndicatorService _service;

        public StaffingIndicatorController(IStaffingIndicatorService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.StaffingIndicatorView)]
        public PagedResult<StaffingIndicatorListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.StaffingIndicatorView)]
        [ProducesResponseType(typeof(StaffingIndicatorDto),200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.StaffingIndicatorView)]
        [ProducesResponseType(typeof(StaffingIndicatorDto), 200)]
        public IActionResult Get(int id)
        {
            StaffingIndicatorDto dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList(bool filterByOrganizationalStructure = true)
        {
            return Ok(_service.AsSelectList(filterByOrganizationalStructure));
        }

        [HttpPost]
        [Authorize(ModuleCode.StaffingIndicatorCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreateStaffingIndicatorDlDto dto)
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
        [Authorize(ModuleCode.StaffingIndicatorEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateStaffingIndicatorDlDto dto)
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
        [Authorize(ModuleCode.StaffingIndicatorDelete)]
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
