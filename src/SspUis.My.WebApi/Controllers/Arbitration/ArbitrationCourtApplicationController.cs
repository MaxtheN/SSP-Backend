using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ArbitrationCourtApplicationController : WebaseController
    {
        private IArbitrationCourtApplicationService _service;

        public ArbitrationCourtApplicationController(IArbitrationCourtApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        public PagedResult<ArbitrationCourtApplicationListDto> GetList([FromBody] ArbitrationCourtApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetCount()
        {
            var result = _service.GetCount();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ArbitrationCourtApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = await _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArbitrationCourtApplicationDto), 200)]
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
        public IActionResult Create(CreateArbitrationCourtApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> res = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateArbitrationCourtApplicationDlDto dto)
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
