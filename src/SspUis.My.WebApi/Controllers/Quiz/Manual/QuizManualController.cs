using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Quiz;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Quiz.Manual
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class QuizManualController : ControllerBase
    {
        private readonly IQuizManualService _service;
        public QuizManualController(IQuizManualService service)
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
}
