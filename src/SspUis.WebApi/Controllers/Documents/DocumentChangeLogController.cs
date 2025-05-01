using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.DocumentChangeLogServices;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentChangeLogController : Controller
    {
        private IDocumentChangeLogService _service;

        public DocumentChangeLogController(IDocumentChangeLogService service)
        {
            _service = service;
        }

        [HttpGet("{tableId}/{docId}")]
        [ProducesResponseType(typeof(List<DocumentChangeLogListDto>), 200)]
        public IActionResult GetListByDocumentId(int tableId, long docId)
        {
            return Ok(_service.GetListByDocumentId(tableId, docId));
        }
    }
}
