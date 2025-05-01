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
    public class SrvApplicationYearlyPlanController : WebaseController
    {
        private ISrvApplicationYearlyPlanService _service;

        public SrvApplicationYearlyPlanController(ISrvApplicationYearlyPlanService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvApplicationYearlyPlanViewAll, ModuleCode.SrvApplicationYearlyPlanView)]
        public PagedResult<SrvApplicationYearlyPlanListDto> GetList([FromBody] SrvApplicationYearlyPlanSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.SrvApplicationYearlyPlanView)]
        [ProducesResponseType(typeof(SrvApplicationYearlyPlanDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [HttpGet]
        [ProducesResponseType(typeof(SrvApplicationYearlyPlanDto), 200)]
        public IActionResult ConvertToCellTables(int? regionId)
        {
            return Ok(_service.ConvertToCellTables(null, regionId.Value));
        }
        [HttpGet("{id}")]
        [Authorize(ModuleCode.SrvApplicationYearlyPlanView)]
        [ProducesResponseType(typeof(SrvApplicationYearlyPlanDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvApplicationYearlyPlanViewAll)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList()
        {
            return Ok(_service.AsSelectList());
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvApplicationYearlyPlanCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateSrvApplicationYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.SrvApplicationYearlyPlanEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateSrvApplicationYearlyPlanDlDto dto)
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
        [Authorize(ModuleCode.SrvApplicationYearlyPlanAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(UpdateStatusSrvApplicationYearlyPlanDto dto)
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
        [Authorize(ModuleCode.SrvApplicationYearlyPlanCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(UpdateStatusSrvApplicationYearlyPlanDto dto)
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
        [Authorize(ModuleCode.SrvApplicationYearlyPlanDelete)]
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