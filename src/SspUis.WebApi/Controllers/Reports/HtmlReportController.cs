using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core.Security;
using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers
{
    //[Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class HtmlReportController : WebaseController
    {
        private IHtmlReportService _service;
        private readonly IAuthService _authService;
        private readonly IWordPrintService _wordPrintService;

        public HtmlReportController(
            IHtmlReportService service,
            IAuthService authService,
            IWordPrintService wordPrintService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _authService = authService;
            _wordPrintService = wordPrintService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public IActionResult TestPdf()
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.TestPdf();
                //var bytes = _wordPrintService.ConvertWordStreamToPdf();
                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public IActionResult Print([FromBody] PdfPrintDto dto)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.Print(dto);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrintPrtnContractPdf(Guid Id2, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPrtnContractPdf(Id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetPrtnContractAsHtml(Guid Id2)
        {
            if (ModelState.IsValid)
            {
                var res = _service.DownloadPrtnContractAsHtml(Id2);

                if (_service.IsValid)
                    return Content(res, "text/html");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

    }
}
