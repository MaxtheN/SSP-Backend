using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("public/[controller]/[action]")]
    public class DocumentChatController : WebaseController
    {
        private readonly IDocumentChatService _service;
        public DocumentChatController(IDocumentChatService service)
            => _service = service;

        [HttpPost]
        public PagedResult<DocumentChatListDto> GetList([FromBody] DocumentChatListDtoSortFilterPageOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateDocumentChatDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
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
}
