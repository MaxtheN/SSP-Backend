using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("memship/[controller]/[action]")]
public class DebtController : WebaseController
{
    private IDebtService _service;

    public DebtController(IDebtService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.DebtView)]
    [ProducesResponseType(typeof(PagedResult<DebtListDto>), 200)]
    public IActionResult GetList([FromBody] DebtSortFilterOption dto)
    {
        if (_service.IsValid)
            return Ok(_service.GetList(dto));

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [Authorize(ModuleCode.DebtView)]
    [ProducesResponseType(typeof(DebtDto), 200)]
    public IActionResult Get()
    {
        if (_service.IsValid)
            return Ok(_service.Get());

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.DebtView)]
    [ProducesResponseType(typeof(DebtDto), 200)]
    public IActionResult Get(long id)
    {
        DebtDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.DebtCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateDebtDlDto dto)
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
    [Authorize(ModuleCode.DebtEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateDebtDlDto dto)
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

    [HttpPost("{id}")]
    [Authorize(ModuleCode.DebtDelete)]
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
