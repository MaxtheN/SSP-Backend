using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Hrm;

[ApiController]
[Authorize]
[Route("hrm/[controller]/[action]")]
public class EmployeeSickLeaveController : WebaseController
{
    private readonly IEmployeeSickLeaveService _service;

    public EmployeeSickLeaveController(IEmployeeSickLeaveService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }


    [HttpPost]
    [Authorize(ModuleCode.EmployeeSickLeaveViewAll)]
    public PagedResult<EmployeeSickLeaveListDto> GetList([FromBody] EmployeeLeaveOrderSortFilter options)
    {
        return _service.GetList(options);
    }
    //[HttpPost]
    //[Authorize(ModuleCode.EmployeeLeaveOrderView)]
    //public PagedResult<UnpaidEmployeeSickLeaveListDto> GetUnpaidList([FromBody] UnpaidEmployeeSickLeaveSortFilterPageOptions dto)
    //{
    //    return _service.GetUnPaidList(dto);
    //}
    [HttpGet]
    [Authorize(ModuleCode.EmployeeSickLeaveView)]
    [ProducesResponseType(typeof(EmployeeSickLeaveDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.EmployeeSickLeaveView, ModuleCode.SignerView)]
    [ProducesResponseType(typeof(EmployeeSickLeaveDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(int? employeeId = null)
    {
        return Ok(_service.AsSelectList(employeeId));
    }
    //[HttpGet]
    //[ProducesResponseType(
    //      type: typeof(IEnumerable<SickLeaveInfoDto>),
    //      statusCode: 200)]
    //public async Task<IActionResult> GetInfoFromHrMf(string pinfl)
    //{
    //    var result
    //        = await _service.GetSickLeaveInfoFromHrMf(pinfl);

    //    if (result != null && result.Any())
    //        return Ok(result);

    //    return Ok();
    //}
    [HttpPost]
    [Authorize(ModuleCode.EmployeeSickLeaveCreate, ModuleCode.AllEmployeeSickLeaveCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateEmployeeSickLeaveDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<long> result = _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeSickLeaveEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateEmployeeSickLeaveDlDto dto)
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
    [Authorize(ModuleCode.EmployeeSickLeaveAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Касаллик варақаси ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusEmployeeSickLeaveDto dTo)
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
    [Authorize(ModuleCode.EmployeeSickLeaveCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Касаллик варақаси ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusEmployeeSickLeaveDto dTo)
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
    [HttpPost("{id}")]
    [Authorize(ModuleCode.EmployeeSickLeaveDelete)]
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
