using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("hrm/[controller]/[action]")]
public class DepartmentController : WebaseController
{
    private IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.DepartmentViewAll)]
    public PagedResult<DepartmentListDto> GetList([FromBody] TableSortFilterPageOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.DepartmentView)]
    [ProducesResponseType(typeof(DepartmentDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.DepartmentView)]
    [ProducesResponseType(typeof(DepartmentDto), 200)]
    public IActionResult Get(int id)
    {
        DepartmentDto dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SelectList<int>), 200)]
    public IActionResult GetAsSelectList(int? organizationId = null)
    {
        var data = _service.AsSelectList(organizationId);
        return Ok(data);
    }

    [HttpPost]
    [Authorize(ModuleCode.DepartmentCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create([FromBody]CreateDepartmentDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<int> result = _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.DepartmentEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateDepartmentDlDto dto)
    {
        if(ModelState.IsValid)
        {
            _service.Update(dto);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    [Authorize(ModuleCode.DepartmentDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(int id)
    {
        if(ModelState.IsValid)
        {
            _service.Delete(id);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
