using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.QuestionnaireService;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Quiz.Info;
[ApiController]
[Route("[controller]/[action]")]
public class QuestionnarieController : WebaseController
{
    private readonly IQuestionnaireService _service;
    public QuestionnarieController(IQuestionnaireService service)
         : base(AppSettings.Instance.ControllerConfig)
        => _service = service;

    [HttpPost]
    public PagedResult<QuestionnaireListDto> GetList([FromBody] QuestionnaireSortFilterOptionsDto dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpGet]
    [ProducesResponseType(typeof(QuestionnaireDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(QuestionnaireDto), 200)]
    public IActionResult Get(long id)
    {
        if (ModelState.IsValid)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
