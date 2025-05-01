using SspUis.BizLogicLayer.AppErrorServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class AppErrorController : WebaseController
{
    private IAppErrorService _service;

    public AppErrorController(IAppErrorService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PagedResult<AppErrorListDto>), 200)]
    public PagedResult<AppErrorListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.AppErrorView)]
    [ProducesResponseType(typeof(AppErrorDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.AppErrorView)]
    [ProducesResponseType(typeof(AppErrorDto), 200)]
    public IActionResult Get(int id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    [AllowAnonymous]
    public IActionResult Create(CreateAppErrorDlDto dto)
    {
        return Ok(); // bu api mobile errors ni post qilishi uchun edi, middleware da user agent bn ajratiladi endi

        //if (ModelState.IsValid)
        //{
        //    HaveId<long> result = _service.Create(dto);

        //    if (_service.IsValid)
        //        return Ok(result);

        //    _service.CopyErrorsToModelState(ModelState);
        //}

        //return ValidationProblem(ModelState);
    }
}
