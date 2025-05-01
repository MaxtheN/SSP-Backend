using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core.Security;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("HrmSigner/[action]")]
    public class DocumentHeldForSignController : WebaseController
    {
        private readonly IDocumentHeldForSignService _service;
        public DocumentHeldForSignController(IDocumentHeldForSignService service)
            => _service = service;
        [HttpPost]
        [Authorize(ModuleCode.SignerView)]
        public IActionResult GetData([FromBody] DocumentHeldForSignFilterOption dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetData(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
