using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{


    [Authorize]
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class SrvYearlyPlanController : WebaseController
    {
        private ISrvYearlyPlanService _service;

        public SrvYearlyPlanController(ISrvYearlyPlanService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvYearlyPlanViewAll)]
        public PagedResult<SrvYearlyPlanListDto> GetList([FromBody] SrvYearlyPlanSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.SrvYearlyPlanView)]
        [ProducesResponseType(typeof(SrvYearlyPlanDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpGet]
        [ProducesResponseType(typeof(SrvYearlyPlanDto), 200)]
        public IActionResult FillTable(int? regionId)
        {
            return Ok(_service.FillTable(regionId.Value));
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SrvYearlyPlanView)]
        [ProducesResponseType(typeof(SrvYearlyPlanDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvYearlyPlanViewAll)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvYearlyPlanCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateSrvYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.SrvYearlyPlanEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateSrvYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.SrvYearlyPlanAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(UpdateStatusSrvYearlyPlanDto dto)
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
        [Authorize(ModuleCode.SrvYearlyPlanCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(UpdateStatusSrvYearlyPlanDto dto)
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
        [Authorize(ModuleCode.SrvYearlyPlanDelete)]
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