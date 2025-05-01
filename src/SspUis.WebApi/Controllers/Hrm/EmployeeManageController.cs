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
using SspUis.BizLogicLayer.Hrm.EmployeeManageServices;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class EmployeeManageController : WebaseController
    {
        private IEmployeeManageService _service;

        public EmployeeManageController(IEmployeeManageService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize(ModuleCode.EmployeeManageView, ModuleCode.AllEmployeeManageView, ModuleCode.ForOtherEmployeeManageView)]
        public PagedResult<EmployeeManageListDto> GetList([FromBody] EmployeeManageSortFilterPageOptions dto)
        {
            return _service.GetList(dto);
        }

        //[HttpGet]
        //[Authorize(ModuleCode.EmployeeManageView)]
        //[ProducesResponseType(typeof(EmployeeManageDto),200)]
        //public IActionResult Get()
        //{
        //    return Ok(_service.Get());
        //}
        [HttpGet]
        //[Authorize(ModuleCode.EmployeeManageView)]
        [ProducesResponseType(typeof(EmployeeCheckForSingDto), 200)]
        public IActionResult CheckSigners()
        {
            return Ok(_service.CheckSigners());
        }
        [HttpGet("{id}")]
        [Authorize(ModuleCode.EmployeeManageView)]
        [ProducesResponseType(typeof(EmployeeManageDto), 200)]
        public IActionResult Get(int id)
        {
            EmployeeManageDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        //[HttpGet()]
        //[ProducesResponseType(typeof(SelectList<int>), 200)]
        //public IActionResult GetAsSelectList(int? employeeManageId = null)
        //{
        //    return Ok(_service.AsSelectList(employeeManageId));
        //}

        //[HttpPost]
        //[Authorize(ModuleCode.EmployeeManageCreate)]
        //[ProducesResponseType(typeof(HaveId<int>), 200)]
        //public IActionResult Create(CreateEmployeeManageDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        HaveId<int> result = _service.Create(dto);

        //        if (_service.IsValid)
        //            return Ok(result);

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost]
        //[Authorize(ModuleCode.WorkScheduleEdit)]
        //[ProducesResponseType(200)]
        //public IActionResult Update(UpdateEmployeeManageDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Update(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost("{id}")]
        //[Authorize(ModuleCode.EmployeeManageDelete)]
        //[ProducesResponseType(200)]
        //public IActionResult Delete(int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Delete(id);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
    }
}
