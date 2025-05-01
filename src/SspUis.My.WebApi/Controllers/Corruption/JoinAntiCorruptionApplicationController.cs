using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.JoinAntiCorruptionApplicationServices;
using SspUis.Core.Configurations;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class JoinAntiCorruptionApplicationController : WebaseController
    {
        private IJoinAntiCorruptionApplicationService _service;
        private readonly SystemConf _systemConf;

        public JoinAntiCorruptionApplicationController(IJoinAntiCorruptionApplicationService service, SystemConf systemConf)
          : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
        }

        [HttpPost]
        [Authorize]
        public PagedResult<JoinAntiCorruptionApplicationListDto> GetList([FromBody] JoinAntiCorruptionApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(JoinAntiCorruptionApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(JoinAntiCorruptionApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateJoinAntiCorruptionApplicationDlDto dto)
        {
            var result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateJoinAntiCorruptionApplicationDlDto dto)
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

        [HttpGet]
        [Authorize]
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

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult Send(SendStatusJoinAntiCorruptionApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Send(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(200)]
        public IActionResult Revoke(RevokeStatusJoinAntiCorruptionApplicationDto dto)
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
