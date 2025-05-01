using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.BizLogicLayer.ReportServices;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnCertificateController : WebaseController
    {
        private IPrtnCertificateService _service;
        private readonly IHtmlReportService _htmlReportService;

        public PrtnCertificateController(
            IPrtnCertificateService service,
            IHtmlReportService htmlReportService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _htmlReportService = htmlReportService;
        }
        [HttpPost]
        public PagedResult<PrtnCertificateListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PrtnCertificateDto), 200)]
        public IActionResult Get(long id)
        {
            PrtnCertificateDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

		[HttpPost]
		[Authorize]
		public IActionResult GetCount()
		{
			int result = _service.GetCount();
			return Ok(result);
		}

		[HttpGet()]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrintCertificatePdf([FromQuery] Guid Id2, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var bytes = _htmlReportService.DownloadCertificatePdf(Id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCertificateAsHtml([FromQuery] Guid Id2)
        {
            if (ModelState.IsValid)
            {
                var res = _htmlReportService.DownloadCertificateAsHtml(Id2);

                if (_service.IsValid)
                    return Content(res, "text/html");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

    }
}
