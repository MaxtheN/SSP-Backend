using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Hrm;

[ApiController]
[Authorize]
[Route("hrm/[controller]/[action]")]
public class WorkDayOffController : WebaseController
{
    private readonly IWorkDayOffService _service;

    public WorkDayOffController(IWorkDayOffService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffViewAll)]
    public PagedResult<WorkDayOffListDto> GetList([FromBody] WorkDayOffSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.WorkDayOffView)]
    [ProducesResponseType(typeof(WorkDayOffDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.WorkDayOffView)]
    [ProducesResponseType(typeof(WorkDayOffDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(WorkDayOffSortFilterOptions options)
    {
        return Ok(_service.AsSelectList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async ValueTask<IActionResult> Create(CreateWorkDayOffDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<long> result = await _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateWorkDayOffDlDto dto)
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
    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffCancel)]
    [ProducesResponseType(200)]
    public IActionResult Cancel(UpdateStatusWorkDayOffDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.Cancel(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.WorkDayOffSign)]
    public async Task<IActionResult> Sign(SignStatusWorkDayOffDto dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Sign(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [Authorize(ModuleCode.WorkDayOffSign)]
    [HttpPost]
    public async ValueTask<IActionResult> WebImzoSign(WebImzoSignedFilter filter)
    {
        if (ModelState.IsValid)
        {
            var url = await _service.SendUrl(filter.Id);

            if (_service.IsValid)
                return Ok(new { Url = url });

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    [Authorize(ModuleCode.WorkDayOffDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(long id)
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
