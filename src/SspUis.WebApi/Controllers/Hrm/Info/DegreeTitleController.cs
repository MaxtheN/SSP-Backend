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
using SspUis.BizLogicLayer.Hrm;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("hrm/[controller]/[action]")]
public class DegreeTitleController : WebaseController
{
    private IDegreeTitleService _service;

    public DegreeTitleController(IDegreeTitleService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.DegreeTitleView)]
    public PagedResult<DegreeTitleListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.DegreeTitleView)]
    [ProducesResponseType(typeof(DegreeTitleDto),200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.DegreeTitleView)]
    [ProducesResponseType(typeof(DegreeTitleDto), 200)]
    public IActionResult Get(int id)
    {
        DegreeTitleDto dto = _service.Get(id);

        if(_service.IsValid)
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
    [Authorize(ModuleCode.DegreeTitleCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create(CreateDegreeTitleDlDto dto)
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
    [Authorize(ModuleCode.DegreeTitleEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateDegreeTitleDlDto dto)
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
    [Authorize(ModuleCode.DegreeTitleDelete)]
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
