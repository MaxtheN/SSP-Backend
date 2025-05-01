using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Doc.PrtnContractServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Job.BizLogicLayer.Services;
using SspUis.Core.Configurations;
using WEBASE.Integration.MSPD.Sud;
using WEBASE.AspNet.Security;
using SspUis.BizLogicLayer.Doc.ApplicationServices;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ApplicationController : WebaseController
    {
        private IApplicationService _service;
        private readonly IHtmlReportService _htmlReportService;
        private readonly IApplicationJobService _jobService;
        private readonly SystemConf _systemConf;

        public ApplicationController(
            IApplicationService service,
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
        [Authorize]
        public PagedResult<BizLogicLayer.ApplicationServices.ApplicationListDto> GetList([FromBody] PrtnDocumentSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetCount()
        {
           int result = _service.GetCount();
           return Ok(result);
        }
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PrtnApplicationDto), 200)]
        public async Task<IActionResult> GetPrtnApplication()
        {
            var dto = await _service.GetPrtnApplication();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        [Authorize]
        [ProducesResponseType(typeof(ApplicationNotificationDto), 200)]
        [HttpGet]
        public IActionResult GetApplicationNotification()
        {
            return Ok(_service.GetApplicationNotification());
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PrtnApplicationDto), 200)]
        public IActionResult GetPrtnApplication(long id)
        {
            var dto = _service.GetPrtnApplication(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApplicationVacanсiesDto), 200)]
        public IActionResult GetApplicationVacancies()
        {
            var result = _service.GetApplicationVacancies();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }


        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(MemshipPaymentsInfo), 200)]
        public IActionResult GetMemshipPayments()
        {
            var result = _service.GetMemshipPayments();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        //[Authorize]
        [ProducesResponseType(typeof(AvailableBenefits), 200)]
        public IActionResult GetBenefits()
        {
            var result = _service.GetAvailableBenefits();
   

            if (_service.IsValid)
                return Ok(result);

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

        [Authorize]
        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult CreatePrtnApplication(CreatePrtnApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.CreatePrtnApplication(dto);

                if (_service.IsValid)
                {
                    if (!_systemConf.IsTest)
                    {
                        _jobService.RequestToSentForReview(result.Id);
                        
                        if (_jobService.IsValid)
                            return Ok(result?.Id);

                        _jobService.CopyErrorsToModelState(ModelState);
                    }

                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Boolean), 200)]
        public IActionResult CanCreateApplication(string inn)
        {
            if (ModelState.IsValid)
            {
                var result = _service.CanCreateApplication(inn);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult UpdatePrtnApplication(UpdatePrtnApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.UpdatePrtnApplication(dto);

                if (_service.IsValid)
                {
                    if (!_systemConf.IsTest)
                    {
                        _jobService.RequestToSentForReview(dto.Id);
                    }

                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
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


        [Authorize]
        [HttpPost("{typeName}")]
        public IActionResult IsRead(string typeName, long Id) 
        {
            var data = _service.IsRead(typeName, Id);
            return Ok(data);
        }

        [HttpPost("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult SentForReview(long id)
        {
            if (ModelState.IsValid)
            {
                if (!_systemConf.IsTest)
                {
                    _jobService.RequestToSentForReview(id);
                }

                _jobService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult Revoke(long id)
        {
            if (ModelState.IsValid)
            {
                _service.Revoke(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PrintApplicationPdf(Guid id, string lang = null!)
        {
            if (ModelState.IsValid)
            {
                var bytes = _htmlReportService.DownloadApplicationPdf(id, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetApplicationAsHtml(long Id)
        {
            if (ModelState.IsValid)
            {
                var res = _htmlReportService.DownloadApplicationAsHtml(Id);

                if (_service.IsValid)
                    return Content(res, "text/html");
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }




        // Mahallaga bittada POST qildik
        [HttpPost]
        [ProducesResponseType(typeof(List<long>), 200)]
        public IActionResult PostMfyOldApplications()
        {
            if (ModelState.IsValid)
            {
                var ids = _service.PostAllOldMfyApplications();

                if (!_systemConf.IsTest)
                {
                    foreach (var id in ids)
                    {
                        _jobService.RequestToSentForReview(id);
                    }
                }

                if (_service.IsValid)
                    return Ok(ids);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

    }
}
