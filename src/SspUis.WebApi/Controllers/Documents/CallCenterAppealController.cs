using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
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
    public class CallCenterAppealController : WebaseController
    {
        private ICallCenterAppealService _service;
        private readonly IEdocRegistrateService _edocRegistrateService;

        public CallCenterAppealController(ICallCenterAppealService service, IEdocRegistrateService edocRegistrateService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
            _edocRegistrateService = edocRegistrateService;
        }

        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealView)]
        public PagedResult<CallCenterAppealListDto> GetList([FromBody] CallCenterAppealSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }
        [HttpGet]
        [Authorize(ModuleCode.CallCenterAppealView)]
        [ProducesResponseType(typeof(CallCenterAppealDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.CallCenterAppealView)]
        [ProducesResponseType(typeof(CallCenterAppealDto), 200)]
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
        public IActionResult GetAsSelectList(CallCenterAppealSortFilterOptions options)
        {
            return Ok(_service.AsSelectList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Create(CreateCallCenterAppealDlDto dto)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> result = _service.Create(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealEdit)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Update(UpdateCallCenterAppealDlDto dto)
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
        [Authorize(ModuleCode.CallCenterAppealDelete)]
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

        //[HttpPost]
        //[Authorize(ModuleCode.CallCenterAppealSign)]
        //public async Task<IActionResult> Sign(SignStatusCallCenterAppealDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _service.Sign(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}
        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealAccept)]
        public async Task<IActionResult> Accept(AcceptStatusCallCenterAppealDto dto)
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
        [Authorize(ModuleCode.CallCenterAppealSendToEdoc)]
        public async Task<IActionResult> SendToEdoc(long id)
        {
            if (ModelState.IsValid)
            {
                await _service.SendToEdoc(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.CallCenterAppealReject)]
        public async Task<IActionResult> Reject(RejectStatusCallCenterAppealDto dto)
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
        [Authorize(ModuleCode.CallCenterAppealDelete)]
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
        public IActionResult PrinAppealApplicationExcel(CallCenterAppealSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.PrinCallCenterAppealExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CallCenterAppealExcel.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}