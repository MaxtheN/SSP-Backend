using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnContractController : WebaseController
    {
        private IPrtnContractService _service;
        private readonly IHtmlReportService _htmlReportService;

        public PrtnContractController(
            IPrtnContractService service,
            IHtmlReportService htmlReportService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _htmlReportService = htmlReportService;
        }
        [HttpPost]
        public PagedResult<PrtnContractListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult Get(long id)
        {
            PrtnContractDto dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> Sign(SignStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Sign(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult Reject(RejectStatusPrtnContractDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

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
                var bytes = _htmlReportService.DownloadPrtnContractPdf(Id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

		[HttpPost]
		[Authorize]
		public IActionResult GetCount()
		{
			int result = _service.GetCount();
			return Ok(result);
		}
		[HttpGet]
        [AllowAnonymous]
        public IActionResult GetPrtnContractAsHtml(Guid Id2)
        {
            if (ModelState.IsValid)
            {
                var res = _htmlReportService.DownloadPrtnContractAsHtml(Id2);

                if (_service.IsValid)
                    return Content(res, "text/html");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
