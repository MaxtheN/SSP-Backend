using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.BizLogicLayer.PrtnContractServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Job.BizLogicLayer.Services;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrtnCertificateController : WebaseController
    {
        private IPrtnCertificateService _service;
        private readonly SystemConf _systemConf;
        private readonly IHtmlReportService _htmlReportService;
        private readonly IPrtnCertificateJobService _jobService;

        public PrtnCertificateController(
            IPrtnCertificateService service,
            IHtmlReportService htmlReportService,
            SystemConf systemConf,
            IPrtnCertificateJobService jobService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _htmlReportService = htmlReportService;
            _systemConf = systemConf;
            _jobService = jobService;
        }
        [HttpPost]
        [Authorize(ModuleCode.PrtnCertificateView)]
        public PagedResult<PrtnCertificateListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.PrtnCertificateView)]
        [ProducesResponseType(typeof(PrtnCertificateDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{prtnContractId}")]
        [Authorize(ModuleCode.PrtnCertificateCreate)]
        [ProducesResponseType(typeof(PrtnContractDto), 200)]
        public IActionResult GetByPrtnContractId(long prtnContractId)
        {
            var dto = _service.GetByPrtnContractId(prtnContractId);

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
        [Authorize(ModuleCode.PrtnCertificateCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreatePrtnCertificateDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                {
                    if (!_systemConf.IsTest)
                    {
                        _jobService.RequestToSent(result.Id);
                    }
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnCertificateCancel)]
        public async Task<IActionResult> Cancel(CancelStatusPrtnCertificateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
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
        [HttpPost]
        public IActionResult SaveAsExecel(PrtnDocumentSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.SaveAsExecel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrtnCertificateTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult PrinGraphExcel(PrtnDocumentSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.PrinGraphExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrintGraphExcel.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.PrtnCertificateView)]
        public IActionResult GetHtmlTemplate(PrtnCertificateDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetHtmlTemplate(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetPdfTemplate(PrtnCertificateDto dto)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetPdfTemplate(dto);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPdfTemplateByPrtnContractId(long id)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetPdfTemplateByPrtnContractId(id);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);

        }

        [HttpGet("{id2}")]
        [AllowAnonymous]
        public IActionResult GetPdf(Guid id2)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetPdfById2(id2);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);

        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetPdfById(long id)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetPdfById(id);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);

        }

        //[HttpPost]
        //[Authorize(ModuleCode.PrtnCertificateEdit)]
        //[ProducesResponseType(200)]
        //public IActionResult Update(UpdatePrtnCertificateDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Update(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        //[HttpPost("{id}")]
        //[Authorize(ModuleCode.PrtnCertificateDelete)]
        //[ProducesResponseType(200)]
        //public IActionResult Delete(long id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Delete(id);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        // PostAllOldCertificates bittada POST qildik
        [HttpPost]
        [ProducesResponseType(typeof(List<long>), 200)]
        public IActionResult PostAllOldCertificates([FromQuery] int id)
        {
            if (ModelState.IsValid)
            {
                var cerIds = _service.PostAllOldCertificates(id);

                if (!_systemConf.IsTest)
                {
                    foreach (var cerId in cerIds)
                    {
                        _jobService.RequestToSent(cerId);
                    }
                }

                if (_service.IsValid)
                    return Ok(cerIds);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
