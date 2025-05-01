using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.QuestionnaireService;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class QuestionnarieController : WebaseController
{
    private readonly IQuestionnaireService _service;

    public QuestionnarieController(IQuestionnaireService service)
         : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.QuestionnaireCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateQuestionnaireDlDto dto)
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

    [HttpGet]
    [Authorize(ModuleCode.QuestionnaireView)]
    [ProducesResponseType(typeof(QuestionnaireDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.QuestionnaireView)]
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

    [HttpPost]
    [Authorize(ModuleCode.QuestionnaireEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateQuestionnaireDlDto dto)
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
}
