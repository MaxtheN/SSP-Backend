using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers.Srv.Doc
{
    [Authorize]
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServiceApplicationController : WebaseController
    {
        private ISrvApplicationService _service;
        private readonly SystemConf _systemConf;
        public ServiceApplicationController(ISrvApplicationService service, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(SrvApplicationSortFilterOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        public PagedResult<SrvApplicationListDto> GetList([FromBody] SrvApplicationSortFilterOptions dto)
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

		[HttpPost]
		[Authorize]
		public IActionResult GetPayedCount()
		{
			int result = _service.GetPayedCount();
			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public IActionResult GetFreeCount()
		{
			int result = _service.GetFreeCount();
			return Ok(result);
		}
		[HttpGet]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            SrvApplicationDto? dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id2}")]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult GetById2(Guid id2)
        {
            var dto = _service.Get(id2);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        public IActionResult GetStatistics()
        {
            var data = _service.GetStatisticsDto();
            if (_service.IsValid)
                return Ok(data);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> CreateAsync(CreateServiceApplicationDlDto dto)
        {
            var res = await _service.CreateSrv(dto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateServiceApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendAsync(SendStatusSrvApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.SendAsync(dto, false);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[ProducesResponseType(200)]
        //public IActionResult AcceptForFree(RecievedStatusSrvApplicationDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Accept(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Reject(RejectStatusSrvApplicationDto dto)
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
        public IActionResult DownloadPdf(Guid id2, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
