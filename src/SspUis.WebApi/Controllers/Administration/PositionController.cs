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
using SspUis.BizLogicLayer.PositionServices;
using SspUis.BizLogicLayer.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Position/[action]")]
    public class PositionController : WebaseController
    {
        private IPositionService _service;

        public PositionController(IPositionService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.PositionView, ModuleCode.AllPositionView)]
        public PagedResult<PositionListDto> GetList([FromBody] SortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.PositionView, ModuleCode.AllPositionView)]
        [ProducesResponseType(typeof(PositionDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.PositionView, ModuleCode.AllPositionView)]
        [ProducesResponseType(typeof(PositionDto), 200)]
        public IActionResult Get(int id)
        {
            PositionDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        //[HttpGet()]
        //[ProducesResponseType(typeof(SelectList<int>), 200)]
        //public IActionResult AllAsSelectList()
        //{
        //    return Ok(_service.AllAsSelectList());
        //}

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectList(bool fromOrganizationalStructure = false)
        {
            return Ok(_service.AsSelectList(fromOrganizationalStructure));
        }

        [HttpPost]
        [Authorize(ModuleCode.PositionCreate, ModuleCode.AllPositionCreate)]
        [ProducesResponseType(typeof(HaveId<int>), 200)]
        public IActionResult Create(CreatePositionDlDto dto)
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
        [Authorize(ModuleCode.PositionEdit, ModuleCode.AllPositionEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdatePositionDlDto dto)
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
        [Authorize(ModuleCode.PositionDelete, ModuleCode.AllPositionDelete)]
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

        [HttpPost]
        public IActionResult SaveAsExecel(TableSortFilterPageOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Position.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
