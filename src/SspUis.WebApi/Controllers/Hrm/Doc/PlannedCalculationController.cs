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
public class PlannedCalculationController : WebaseController
{
    private readonly IPlannedCalculationService _service;

    public PlannedCalculationController(IPlannedCalculationService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }


    [HttpPost]
    [Authorize(ModuleCode.PlannedCalculationViewAll)]
    public PagedResult<PlannedCalculationListDto> GetList([FromBody] PlannedCalculationSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.PlannedCalculationView)]
    [ProducesResponseType(typeof(PlannedCalculationDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.PlannedCalculationView)]
    [ProducesResponseType(typeof(PlannedCalculationDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.PlannedCalculationViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(PlannedCalculationSortFilterOptions options)
    {
        return Ok(_service.AsSelectList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.PlannedCalculationCreate, ModuleCode.AllPlannedCalculationCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreatePlannedCalculationDlDto dto)
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
    [Authorize(ModuleCode.PlannedCalculationEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdatePlannedCalculationDlDto dto)
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
    [Authorize(ModuleCode.PlannedCalculationDelete)]
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
