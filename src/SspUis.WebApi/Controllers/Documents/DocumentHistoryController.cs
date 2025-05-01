using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.DocumentHistoryService;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentHistoryController : WebaseController
    {
        private IDocumentHistoryService _service;

        public DocumentHistoryController(
            IDocumentHistoryService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpGet("{tableId}/{previouesId}/{currentId}")]
        public IActionResult CompareContent(int tableId, long previouesId, long currentId)
        {
            var result = _service.CompareContents(tableId, previouesId, currentId);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public IActionResult GetLastMessage([FromQuery] DocLastMessageRequestDto dto)
        {
            var result = _service.GetLastMessage(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
