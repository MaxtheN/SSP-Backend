using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.Core.Security;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("HrmEmp/[action]")]
    public class DocumentHeldForEmpController : WebaseController
    {
        private readonly IDocumentHeldForEmpService _service;
        public DocumentHeldForEmpController(IDocumentHeldForEmpService service)
        {
            _service = service;
        }

        [HttpPost]
        //[Authorize(ModuleCode.SignerView)]
        public IActionResult GetByPinflData([FromBody] DocumentHeldForEmpFilterOption dto)
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
