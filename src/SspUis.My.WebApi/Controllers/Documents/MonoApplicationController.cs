using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Job.BizLogicLayer.Services;
using SspUis.Core.Configurations;
using WEBASE.Integration.MSPD.Sud;
using SspUis.BizLogicLayer.MonoApplicationServices;
using WEBASE.Storage;
using SspUis.BizLogicLayer.Doc;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MonoApplicationController : WebaseController
    {
        private IMonoApplicationService _service;
        private readonly IMonoApplicationResultService _resultServcie;
        private readonly IHtmlReportService _htmlReportService;
        private readonly IMonoApplicationJobService _jobService;
        private readonly SystemConf _systemConf;

        public MonoApplicationController(
            IMonoApplicationService service,
            IHtmlReportService htmlReportService,
            IMonoApplicationResultService resultService,
            IMonoApplicationJobService jobService,
            SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _resultServcie = resultService;
            _jobService = jobService;
            _htmlReportService = htmlReportService;
            _systemConf = systemConf;
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetAllMonoAppRes()
        => Ok(_resultServcie.GetList());

        [Authorize]
        [HttpGet]
        public IActionResult GetByMonoAppId(long appId)
        => Ok(_resultServcie.GetAppId(appId));
        [HttpPost]
        [Authorize]
        public PagedResult<BizLogicLayer.MonoApplicationServices.MonoApplicationListDto> GetList([FromBody] MonoApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
		[Authorize]
        public IActionResult GetCount()
        {
            return Ok(_service.GetCount());
        }

		[Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(MonoApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MonoApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult>  Create(CreateMonoApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Create(dto);
                

                if (_service.IsValid)
                {
                    _jobService.RequestToSentForReview(result.Id);
                    if (_jobService.IsValid)
                        return Ok(result?.Id);
                    _jobService.CopyErrorsToModelState(modelState: ModelState);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(Boolean), 200)]
        public IActionResult CanCreate()
        {
            if (ModelState.IsValid)
            {
                var result = _service.CanCreate();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendForReviewWithoutRabbit(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.SentForReview(id);
                    if (_service.IsValid)
                        return Ok();
                    _service.CopyErrorsToModelState(ModelState);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return ValidationProblem(ModelState);

        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendForReviewWithoutRabbitDoniyor2(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _service.SentForReview(id);
                    if (_service.IsValid)
                        return Ok();
                    _service.CopyErrorsToModelState(ModelState);

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return ValidationProblem(ModelState);

        }


        //[Authorize]
        //[HttpPost]
        //[ProducesResponseType(200)]
        //public IActionResult Update(UpdateMonoApplicationDlDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Update(dto);

        //        if (_service.IsValid)
        //        {
        //            //if (!_systemConf.IsTest)
        //            //{
        //            //    _jobService.RequestToSentForReview(dto.Id);
        //            //}

        //            return Ok();
        //        }

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
        [Authorize]
        [HttpPost("{id}")]
        [ProducesResponseType(200)]
        public IActionResult SentForReview(long id)
        {
            if (ModelState.IsValid)
            {
                if (!_systemConf.IsTest)
                    _jobService.RequestToSentForReview(id);

                if(_jobService.IsValid)
                    return Ok();

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

        [HttpPost]
        [ServiceFilter(typeof(UploadFileAttribute))]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult UploadFile([FromForm] List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                StorageFile[] dto = files.Select(a => new StorageFile(a.FileName, a.OpenReadStream())).ToArray();
                var result = _service.UploadFiles(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet]

        public IActionResult TestZip()
        {
            if (ModelState.IsValid)
            {
                _service.Test();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet("{fileId}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public IActionResult DownloadFile(Guid fileId, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                StorageFile file = _service.DownloadFile(fileId);

                if (_service.IsValid)
                    return File(file.GetStream(), mimeMappingService.Map(file.FileName));

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{fileId}")]
        public IActionResult DeleteFile(Guid fileId)
        {
            if (ModelState.IsValid)
            {
                _service.DeleteFile(fileId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadPdf(Guid id2, string? lang = null)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

    }
}
