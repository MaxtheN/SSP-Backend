using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Kpi;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Kpi;

[Route("kpi/[controller]/[action]")]
[ApiController]
[Authorize]
public class KpiGratingController : WebaseController
{
    private IKpiGratingService _service;

    public KpiGratingController(IKpiGratingService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiGratingViewAll)]
    public PagedResult<KpiGratingListDto> GetList([FromBody] KpiGratingSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.KpiGratingView)]
    [ProducesResponseType(typeof(KpiGratingDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }
    [HttpGet]
    [ProducesResponseType(typeof(KpiGratingDto), 200)]
    public IActionResult FillIndicator()
    {
        return Ok(_service.FillIndicator());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.KpiGratingView)]
    [ProducesResponseType(typeof(KpiGratingDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiGratingViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpPost]
    //[Authorize(ModuleCode.KpiGratingCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateKpiGratingDlDto dto)
    {
        if (ModelState.IsValid)
        {
            HaveId<long> result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiGratingEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateKpiGratingDlDto dto)
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
    [Authorize(ModuleCode.KpiGratingAccept)]
    [ProducesResponseType(200)]
    public IActionResult Accept(UpdateStatusKpiGratingDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiGratingCancel)]
    [ProducesResponseType(200)]
    public IActionResult Cancel(UpdateStatusKpiGratingDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Cancel(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost("{id}")]
    [Authorize(ModuleCode.KpiGratingDelete)]
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

