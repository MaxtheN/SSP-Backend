using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Hrm;

[ApiController]
[Authorize]
[Route("hrm/[controller]/[action]")]
public class TimesheetController : WebaseController
{
    private readonly ITimesheetService _service;

    public TimesheetController(ITimesheetService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetViewAll)]
    public PagedResult<TimesheetListDto> GetList([FromBody] TimesheetSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.TimesheetView)]
    [ProducesResponseType(typeof(TimesheetDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.TimesheetView)]
    [ProducesResponseType(typeof(TimesheetDto), 200)]
    public IActionResult Get(long id)
    {
        if (ModelState.IsValid)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }


    [HttpPost]
    [Authorize(ModuleCode.TimesheetView)]
    public IActionResult SaveAsExecelForTabel(long id)
    {
        if (ModelState.IsValid)
        {
            var file = _service.SaveAsExecelForTabel(id);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tabel.xlsx");

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }


    [HttpGet("{id}")]
    [Authorize(ModuleCode.TimesheetView)]
    [ProducesResponseType(typeof(TimesheetTableDto), 200)]
    public IActionResult GetTable(long id)
    {
        if (ModelState.IsValid)
        {
            var dto = _service.GetTable(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [Authorize(ModuleCode.TimesheetViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(int? employeeId = null)
    {
        return Ok(_service.AsSelectList(employeeId));
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetEdit)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult FillTimeSheet([FromBody] TimesheetFillDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _service.FillTimeSheet(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateTimesheetDlDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateTimesheetDlDto dto)
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

    [HttpPost]
    [Authorize(ModuleCode.TimesheetEdit)]
    [ProducesResponseType(200)]
    public IActionResult UpdateTable(TimesheetTableWithDaysDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.UpdateTable(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetAccept)]
    [ProducesResponseType(200)]
    public IActionResult Accept(UpdateStatusTimesheetDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.TimesheetCancel)]
    [ProducesResponseType(200)]
    public IActionResult Cancel(UpdateStatusTimesheetDto dTo)
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
    [Authorize(ModuleCode.TimesheetEdit)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Clear(ClearTableDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _service.ClearTimeSheetTable(dto);

            if (_service.IsValid)
                return Get(dto.Id);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    [Authorize(ModuleCode.TimesheetDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(long id)
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
