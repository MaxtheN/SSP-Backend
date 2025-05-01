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
public class MassPlannedCalculationController : WebaseController
{
    private readonly IMassPlannedCalculationService _service;

    public MassPlannedCalculationController(IMassPlannedCalculationService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.MassPlannedCalculationViewAll)]
    public PagedResult<MassPlannedCalculationListDto> GetList([FromBody] MassPlannedCalculationSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.MassPlannedCalculationView)]
    [ProducesResponseType(typeof(MassPlannedCalculationDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.MassPlannedCalculationView)]
    [ProducesResponseType(typeof(MassPlannedCalculationDto), 200)]
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
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }
    [HttpPost]
    [Authorize(ModuleCode.MassPlannedCalculationCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateMassPlannedCalculationDlDto dto)
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
    [Authorize(ModuleCode.MassPlannedCalculationEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMassPlannedCalculationDlDto dto)
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
    [Authorize(ModuleCode.MassPlannedCalculationAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusMassPlannedCalculationDto dTo)
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
    [Authorize(ModuleCode.MassPlannedCalculationCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusMassPlannedCalculationDto dTo)
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
    [Authorize(ModuleCode.MassPlannedCalculationDelete)]
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
