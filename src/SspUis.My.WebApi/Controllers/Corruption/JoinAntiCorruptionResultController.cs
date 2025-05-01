using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Corruption;
using SspUis.Core.Configurations;
using WEBASE.AspNet;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class JoinAntiCorruptionResultController : WebaseController
    {
        private IJoinAntiCorruptionResultService _service;
        private readonly SystemConf _systemConf;

        public JoinAntiCorruptionResultController(IJoinAntiCorruptionResultService service, SystemConf systemConf)
          : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _systemConf = systemConf;
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
