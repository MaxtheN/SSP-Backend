using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Doc.SignDocumentManage;
using WbImzo.Models;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignDocumentManageController : WebaseController
    {
        private ISignDocumentManageService _service;

        public SignDocumentManageController(ISignDocumentManageService service)
        {
            _service = service;
        }
        //[HttpPost]
        //[BasicAuth("web_imzo")]
        //public async ValueTask<IActionResult> PostSignResponse(WbImzoSignInformDto signInformDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _service.PostSignResponse(signInformDto);

        //        if (_service.IsValid)
        //            return Ok(result);

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [HttpPost]
        [BasicAuth("web_imzo")]
        public async ValueTask<IActionResult> PostSignResponse(WbImzoSignInformDto signInformDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.PostSignResponse2(signInformDto);

                //if (_service.IsValid)
                return Ok(result);

                //  _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("RefreshStatus")]
        [Authorize]
        public async ValueTask<IActionResult> RefreshStatus(UpdateStatusWebImzoDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateStatus(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}