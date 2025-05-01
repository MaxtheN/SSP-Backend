using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi;

[Authorize]
[ApiController]
[Route("Arbitration/[controller]/[action]")]
public class ArbitrationJudgeController : WebaseController
{
    private IArbitrationJudgeService _service;

    public ArbitrationJudgeController(IArbitrationJudgeService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    public PagedResult<ArbitrationJudgeListDto> GetList([FromBody] ArbitrationJudgeSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ArbitrationJudgeDto), 200)]
    public IActionResult Get(int id)
    {
        ArbitrationJudgeDto dto = _service.Get(id);

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
}
