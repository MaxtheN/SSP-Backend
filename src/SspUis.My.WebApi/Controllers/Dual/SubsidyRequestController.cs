using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Configurations;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Dual/[controller]/[action]")]
    public class SubsidyRequestController : WebaseController
    {
        private ISubsidyRequestService _service;
        private readonly SystemConf _systemConf;

        public SubsidyRequestController(ISubsidyRequestService service, SystemConf systemConf)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        public PagedResult<SubsidyRequestListDto> GetList([FromBody] SubsidyRequestSortFilterOption dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SubsidyRequestDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

		[HttpPost]
		public IActionResult GetCount()
		{
			var result = _service.GetCount();
			return Ok(result);
		}

		[HttpGet("{id}")]
        [ProducesResponseType(typeof(SubsidyRequestDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> Create(CreateSubsidyRequestDlDto dto)
        {
            var result = await _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateSubsidyRequestDlDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Send(SendStatusSubsidyRequestDto dto)
        {
            if (ModelState.IsValid)
            {
               await _service.Send(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Revoke(UpdateStatusSubsidyRequestDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Revoke(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
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
    }
}
