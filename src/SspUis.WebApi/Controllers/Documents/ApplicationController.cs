using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.DataLayer.Repositories;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.BizLogicLayer;
using SspUis.Core.Configurations;
using SspUis.Job.BizLogicLayer.Services;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApplicationController : WebaseController
    {
        private readonly IApplicationService _service;
        private readonly IHtmlReportService _htmlReportService;
        private readonly IApplicationJobService _jobService;
        private readonly SystemConf _systemConf;

        public ApplicationController(IApplicationService service,
            IHtmlReportService htmlReportService,
            IApplicationJobService jobService,
            SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _jobService = jobService;
            _htmlReportService = htmlReportService;
            _systemConf = systemConf;
        }
        [HttpPost]
        [Authorize(ModuleCode.ApplicationView)]
        public PagedResult<BizLogicLayer.ApplicationServices.ApplicationListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationView)]
        public IQueryable<BizLogicLayer.ApplicationServices.ApplicationListDto> GetListMethod([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetListMethod(dto);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ApplicationView)]
        [ProducesResponseType(typeof(PrtnApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.GetPrtnApplication(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{applicationId2}")]
        [ProducesResponseType(typeof(MfyApplicationLogDto), 200)]
        public IActionResult GetMfyApplication(Guid applicationId2)
        {
            var dto = _service.GetMfyApplication(applicationId2);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationAccept, ModuleCode.PrtnContractCreateManually)] // Admin tomondan toxtab qogan arizalani CREATE qvorish uchun
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Accept(AcceptStatusPrtnApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ApplicationReject)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> Reject(RejectStatusPrtnApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult SentForReviewFromJob(long id)
        {
            if (ModelState.IsValid)
            {
                if (!_systemConf.IsTest)
                {
                    _jobService.RequestToSentForReview(id);
                    if(_jobService.IsValid)
                        return Ok();
                }


                _jobService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult SentForReview(long id)
        {
            if (ModelState.IsValid)
            {
                if (!_systemConf.IsTest)
                {
                    _service.SentForReview(id);
                }

                //return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult SentToMahalla(long id)
        {
            if (ModelState.IsValid)
            {
                if (!_systemConf.IsTest)
                {
                    _service.SentToMahalla(id);
                }

                //return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrintApplicationPdf(Guid Id, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var bytes = _htmlReportService.DownloadApplicationPdf(Id, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetApplicationAsHtml(long Id, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var res = _htmlReportService.DownloadApplicationAsHtml(Id, lang: lang);

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
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ApplicationTemplate.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AddPrtnApplicationStatusChangeExpOn()
        {
            if (ModelState.IsValid)
            {
                _service.AddPrtnApplicationStatusChangeExpOn();

                if (_service.IsValid)
                    return Ok();
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
