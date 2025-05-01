using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("Memship/[controller]/[action]")]
public class ContractorCategoryCriterionController : WebaseController
{
    private readonly IContractorCategoryCriterionService _service;

    public ContractorCategoryCriterionController(IContractorCategoryCriterionService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.ContractorCategoryCriterionViewAll)]
    public PagedResult<ContractorCategoryCriterionListDto> GetList([FromBody] ContractorCategoryCriterionSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.ContractorCategoryCriterionView)]
    [ProducesResponseType(typeof(ContractorCategoryCriterionDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.ContractorCategoryCriterionView)]
    [ProducesResponseType(typeof(ContractorCategoryCriterionDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
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
    [Authorize(ModuleCode.ContractorCategoryCriterionCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateContractorCategoryCriterionDlDto dto)
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
    [Authorize(ModuleCode.ContractorCategoryCriterionEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateContractorCategoryCriterionDlDto dto)
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
    [Authorize(ModuleCode.ContractorCategoryCriterionAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusContractorCategoryCriterionDto dTo)
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
    [Authorize(ModuleCode.ContractorCategoryCriterionCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusContractorCategoryCriterionDto dTo)
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
    [Authorize(ModuleCode.ContractorCategoryCriterionDelete)]
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
