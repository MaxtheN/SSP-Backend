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
public class ElectionMemberController : WebaseController
{
    private IElectionMemberService _service;

    public ElectionMemberController(IElectionMemberService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.ElectionMemberView)]
    public PagedResult<ElectionMemberListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.ElectionMemberView)]
    [ProducesResponseType(typeof(ElectionMemberDto),200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.ElectionMemberView)]
    [ProducesResponseType(typeof(ElectionMemberDto), 200)]
    public IActionResult Get(int id)
    {
        ElectionMemberDto dto = _service.Get(id);

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
    [Authorize(ModuleCode.ElectionMemberCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create(CreateElectionMemberDlDto dto)
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
    [Authorize(ModuleCode.ElectionMemberEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateElectionMemberDlDto dto)
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
    [Authorize(ModuleCode.ElectionMemberDelete)]
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
