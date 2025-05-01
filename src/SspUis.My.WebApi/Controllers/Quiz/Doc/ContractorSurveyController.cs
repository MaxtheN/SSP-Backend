using Microsoft.AspNetCore.Mvc;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WbCrm.BizLogicLayer.ContractorSurveyServices;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class ContractorSurveyController : WebaseController
{
    private readonly IContractorSurveyService _service;
    public ContractorSurveyController(IContractorSurveyService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateContractorSurveyDlDto dto)
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
    public PagedResult<ContractorSurveyListDto> GetList([FromBody] ContractorSurveySortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.ContractorSurveyView)]
    [ProducesResponseType(typeof(ContractorSurveyDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.ContractorSurveyView)]
    [ProducesResponseType(typeof(ContractorSurveyDto), 200)]
    public IActionResult Get(long id)
    {
        return Ok(_service.Get(id));
    }

    [HttpPost("{id}")]
    [Authorize(ModuleCode.ContractorSurveyDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(int id)
    {
        if (ModelState.IsValid)
        {
            var result = _service.Delete(id);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}
