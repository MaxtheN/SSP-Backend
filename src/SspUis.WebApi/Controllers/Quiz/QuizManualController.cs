using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Quiz;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Quiz;

[Authorize]
[ApiController]
[Route("quiz/[controller]/[action]")]
public class ManualController : ControllerBase
{
    private readonly IQuizManualService _service;

    public ManualController(IQuizManualService service)
    {
        this._service = service;
    }

    [HttpGet]
    public SelectList<int> AnswerTypeSelectList()
    {
        return _service.AnswerTypeSelectList();
    }

    [HttpGet]
    public SelectList<int> InspectionTypeSelectList()
    {
        return _service.InspectionTypeSelectList();
    }

    [HttpGet]
    public SelectList<int> QuestionnaireTypeSelectList()
    {
        return _service.QuestionnaireTypeSelectList();
    }
}
