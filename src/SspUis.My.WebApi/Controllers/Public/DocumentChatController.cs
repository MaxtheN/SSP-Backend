using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentChatController : WebaseController
    {
        private readonly IDocumentChatService _service;
        public DocumentChatController(IDocumentChatService service)
           => _service = service;

        [HttpPost]
        //[Authorize(ModuleCode.SettlementAccountSourceView)]
        public PagedResult<DocumentChatListDto> GetList([FromBody] DocumentChatListDtoSortFilterPageOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        //[Authorize(ModuleCode.SettlementAccountSourceCreate)]
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
        //[Authorize(ModuleCode.SettlementAccountSourceDelete)]
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
