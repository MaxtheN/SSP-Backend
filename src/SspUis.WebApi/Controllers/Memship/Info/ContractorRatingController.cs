
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Memship;


[Authorize]
[ApiController]
[Route("memship/[controller]/[action]")]
public class ContractorRatingController : WebaseController
{
    private IContractorRatingService _service;

    public ContractorRatingController(IContractorRatingService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.ContractorRatingView)]
    public PagedResult<ContractorRatingListDto> GetList([FromBody] SortFilterPageOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.ContractorRatingView)]
    [ProducesResponseType(typeof(ContractorRatingDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.ContractorRatingView)]
    [ProducesResponseType(typeof(ContractorRatingDto), 200)]
    public IActionResult Get(int id)
    {
        ContractorRatingDto dto = _service.GetById(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelectList<int>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpPost]
    [Authorize(ModuleCode.ContractorRatingCreate)]
    [ProducesResponseType(typeof(HaveId<int>), 200)]
    public IActionResult Create([FromBody] CreateContractorRatingDlDto dto)
    {
        if (ModelState.IsValid)
        {
            HaveId<int> result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.ContractorRatingEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateContractorRatingDlDto dto)
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
    [Authorize(ModuleCode.ContractorRatingDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(int id)
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