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
public class MilitaryRankController : WebaseController
{
    private IMilitaryRankService _service;

    public MilitaryRankController(IMilitaryRankService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.MilitaryRankView)]
    public PagedResult<MilitaryRankListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.MilitaryRankView)]
    [ProducesResponseType(typeof(MilitaryRankDto),200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.MilitaryRankView)]
    [ProducesResponseType(typeof(MilitaryRankDto), 200)]
    public IActionResult Get(int id)
    {
        MilitaryRankDto dto = _service.Get(id);

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
    [Authorize(ModuleCode.MilitaryRankCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create(CreateMilitaryRankDlDto dto)
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
    [Authorize(ModuleCode.MilitaryRankEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMilitaryRankDlDto dto)
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
    [Authorize(ModuleCode.MilitaryRankDelete)]
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
