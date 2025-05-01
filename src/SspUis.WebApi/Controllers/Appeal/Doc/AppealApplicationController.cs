using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Appeal;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Appeal;
using SspUis.Integration.Edoc;
using SspUis.Integration.Edoc.Models;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{


    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppealApplicationController : WebaseController
    {
        private IAppealApplicationService _service;
        private readonly IEdocRegistrateService _edocRegistrateService;

        public AppealApplicationController(IAppealApplicationService service, IEdocRegistrateService edocRegistrateService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
            _edocRegistrateService = edocRegistrateService;
        }

        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationView)]
        public PagedResult<AppealApplicationListDto> GetList([FromBody] AppealApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(long), 200)]
        public IActionResult GetCount()
        {
            return Ok(_service.GetCount());
        }

        [HttpGet]
        [Authorize(ModuleCode.AppealApplicationView)]
        [ProducesResponseType(typeof(AppealApplicationDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.AppealApplicationView)]
        [ProducesResponseType(typeof(AppealApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(AppealApplicationSortFilterOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async Task<IActionResult> Create(CreateAppealApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = await _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateAppealApplicationDlDto dto)
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

        [HttpPost("{id}")]
        [Authorize(ModuleCode.AppealApplicationDelete)]
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
        [Authorize(ModuleCode.AppealApplicationSign)]
        public async Task<IActionResult> Sign(SignStatusAppealApplicationDto dto)
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
        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationAccept)]
        public async Task<IActionResult> Accept(AcceptStatusAppealApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Accept(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationSendToEdoc)]
        public async Task<IActionResult> SendToEdoc(long id, int organizationId)
        {
            if (ModelState.IsValid)
            {
                await _service.SendToEdoc(id, organizationId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.AppealApplicationReject)]
        public async Task<IActionResult> Reject(RejectStatusAppealApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.Reject(dto);

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
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
        public async Task<IActionResult> DownloadAttachment(Guid fileId, bool isView, [FromServices] IMimeMappingService mimeMappingService)
        {
            if (ModelState.IsValid)
            {
                var file = await _service.DownloadAttachment(fileId, isView);

                if (_service.IsValid)
                    return Ok(file);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost("{fileId}")]
        [Authorize(ModuleCode.AppealApplicationDelete)]
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
        public async Task<IActionResult> DownloadPdf(long id2, string lang)
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


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegistrateEdoc(EdocRegisterRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _edocRegistrateService.RegistrateEdoc(dto);

                if (_edocRegistrateService.IsValid)
                    return Ok(result);

                _edocRegistrateService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult PrinAppealApplicationExcel(AppealApplicationSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.PrinAppealApplicationExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AppealApplicationExcel.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}