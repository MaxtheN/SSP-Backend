using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("hrm/[controller]/[action]")]
public class EmployeeMissedDayController : WebaseController
{
    private readonly IEmployeeMissedDayService _service;

    public EmployeeMissedDayController(IEmployeeMissedDayService employeeMissedDayService)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = employeeMissedDayService;
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeMissedDayView)]
    public PagedResult<EmployeeMissedDayListDto> GetList(EmployeeMissedDaySortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.EmployeeMissedDayView)]
    [ProducesResponseType(typeof(EmployeeMissedDayDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeMissedDayCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateEmployeeMissedDayDlDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _service.Create(dto, null);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult CreateByEmployee(EmployeeMissedDayTableDlDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _service.CreateByEmployee(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    [Authorize(ModuleCode.EmployeeMissedDayEdit)]
    public IActionResult Update(UpdateEmployeeMissedDayDlDto dto)
    {
        if (ModelState.IsValid)
        {
            var res = _service.Update(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    [ProducesResponseType(200)]
    [Authorize(ModuleCode.EmployeeMissedDayDelete)]
    public IActionResult Delete(long id)
    {
        if (ModelState.IsValid)
        {
            var res = _service.UpdateStatus(new UpdateStatusEmployeeMissedDayDto { Id = id }, StatusIdConst.DELETED);

            if (_service.IsValid)
                return Ok(HaveId.Create(id));

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeMissedDayApprove)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Approve(UpdateStatusEmployeeMissedDayDto dto)
    {
        if (ModelState.IsValid)
        {
            var res = _service.Approve(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeMissedDayCancelApprove)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult CancelApprove(UpdateStatusEmployeeMissedDayDto dto)
    {
        if (ModelState.IsValid)
        {
            var res = _service.CancelApprove(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.EmployeeMissedDayView)]
    [ProducesResponseType(typeof(EmployeeMissedDayDto), 200)]
    public IActionResult Get(long id)
    {
        EmployeeMissedDayDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }
}
