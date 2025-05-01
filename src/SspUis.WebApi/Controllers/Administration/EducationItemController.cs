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
using SspUis.BizLogicLayer;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("EducationItem/[action]")]
public class EducationItemController : WebaseController
{
    private IEducationItemService _service;

    public EducationItemController(IEducationItemService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.EducationItemView)]
    public PagedResult<EducationItemListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.EducationItemView)]
    [ProducesResponseType(typeof(EducationItemDto),200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.EducationItemView)]
    [ProducesResponseType(typeof(EducationItemDto), 200)]
    public IActionResult Get(int id)
    {
        EducationItemDto dto = _service.Get(id);

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
    [Authorize(ModuleCode.EducationItemCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create(CreateEducationItemDlDto dto)
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
    [Authorize(ModuleCode.EducationItemEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateEducationItemDlDto dto)
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
    [Authorize(ModuleCode.EducationItemDelete)]
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
