using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{


    [Authorize]
    [ApiController]
    [Route("memship/[controller]/[action]")]
    public class MemshipYearlyPlanController : WebaseController
    {
        private IMemshipYearlyPlanService _service;

        public MemshipYearlyPlanController(IMemshipYearlyPlanService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipYearlyPlanViewAll)]
        public PagedResult<MemshipYearlyPlanListDto> GetList([FromBody] MemshipYearlyPlanSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.MemshipYearlyPlanView)]
        [ProducesResponseType(typeof(MemshipYearlyPlanDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpGet]
        [ProducesResponseType(typeof(MemshipYearlyPlanDto), 200)]
        public IActionResult FillTable()
        {
            return Ok(_service.FillTable());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.MemshipYearlyPlanView)]
        [ProducesResponseType(typeof(MemshipYearlyPlanDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipYearlyPlanViewAll)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.MemshipYearlyPlanCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateMemshipYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.MemshipYearlyPlanEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateMemshipYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.MemshipYearlyPlanAccept)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини тасдиқлаш")]
        public IActionResult Accept(UpdateStatusMemshipYearlyPlanDto dto)
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
        [Authorize(ModuleCode.MemshipYearlyPlanCancel)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини бекор қилиш")]
        public IActionResult Cancel(UpdateStatusMemshipYearlyPlanDto dto)
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
        [Authorize(ModuleCode.MemshipYearlyPlanDelete)]
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
    }

}