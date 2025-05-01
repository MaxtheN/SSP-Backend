using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Arbitration/[controller]/[action]")]
    public class ArbitrationResultController : WebaseController
    {
        private IArbitrationResultService _service;

        public ArbitrationResultController(IArbitrationResultService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationResultView)]
        public PagedResult<ArbitrationResultListDto> GetList([FromBody] ArbitrationResultSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ArbitrationResultView)]
        [ProducesResponseType(typeof(ArbitrationResultDto), 200)]
        public async Task<IActionResult> GetByArbitrationCourtApplicationId(long arbitrationCourtApplicationId)
        {
            var dto = await _service.GetByArbitrationCourtApplicationId(arbitrationCourtApplicationId);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ArbitrationResultView)]
        [ProducesResponseType(typeof(ArbitrationResultDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationResultDelete)]
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
        [Authorize(ModuleCode.ArbitrationResultCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateArbitrationResultDlDto dto)
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
        [Authorize(ModuleCode.ArbitrationResultEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateArbitrationResultDlDto dto)
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
        [Authorize(ModuleCode.ArbitrationResultCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusArbitrationResultDto dTo)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dTo);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationResultSign)]
        public async Task<IActionResult> Sign(SignStatusArbitrationResultDto dto)
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

        #region Files
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
        [Authorize(ModuleCode.ArbitrationResultDelete)]
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
        #endregion

    }
}
