using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Corruption;
using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Corruption;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("corruption/[controller]/[action]")]
    public class JoinAntiCorruptionResultController : WebaseController
    {
        private IJoinAntiCorruptionResultService _service;

        public JoinAntiCorruptionResultController(IJoinAntiCorruptionResultService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionResultViewAll)]
        public PagedResult<JoinAntiCorruptionResultListDto> GetList([FromBody] JoinAntiCorruptionResultSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.JoinAntiCorruptionResultView)]
        [ProducesResponseType(typeof(JoinAntiCorruptionResultDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.JoinAntiCorruptionResultView)]
        [ProducesResponseType(typeof(JoinAntiCorruptionResultDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionResultViewAll)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionResultCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateJoinAntiCorruptionResultDlDto dto)
        {
            if(ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if(_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionResultEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateJoinAntiCorruptionResultDlDto dto)
        {
            if(ModelState.IsValid)
            {
                _service.Update(dto);

                if(_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        
        [HttpPost]
        [Authorize(ModuleCode.JoinAntiCorruptionResultAccept)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини тасдиқлаш")]
        public IActionResult Accept(UpdateStatusJoinAntiCorruptionResultDto dto)
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
        [Authorize(ModuleCode.JoinAntiCorruptionResultCancel)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини бекор қилиш")]
        public IActionResult Cancel(UpdateStatusJoinAntiCorruptionResultDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Cancel(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        
        [HttpPost("{id}")]
        [Authorize(ModuleCode.JoinAntiCorruptionResultDelete)]
        [ProducesResponseType(200)]
        public IActionResult Delete(long id)
        {
            if(ModelState.IsValid)
            {
                _service.Delete(id);

                if(_service.IsValid)
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
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadEmployeeCV(Guid id2, string? lang)
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