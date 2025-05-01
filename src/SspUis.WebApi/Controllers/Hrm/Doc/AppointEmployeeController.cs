using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.IntegrationServices;
using SspUis.BizLogicLayer.IntegrationServices.Xodim;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("hrm/[controller]/[action]")]
    public class AppointEmployeeController : WebaseController
    {
        private IAppointEmployeeService _service;
        private readonly IMehnatService _mehnatService;
        private readonly IWorkActivity _workActivity;
        private IXodimPhotoUploader _photoUploader;

        public AppointEmployeeController(IAppointEmployeeService service, IMehnatService mehnatService, IWorkActivity workActivity, IXodimPhotoUploader photoUploader)
            : base(AppSettings.Instance.ControllerConfig)
        {
            this._service = service;
            _mehnatService = mehnatService;
            _workActivity = workActivity;
            _photoUploader = photoUploader;
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeView)]
        public PagedResult<AppointEmployeeListDto> GetList([FromBody] AppointEmployeeSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeHeaderView)]
        public PagedResult<AppointEmployeeListDto> GetListForHeader([FromBody] AppointEmployeeSortFilterOptions dto)
        {
            return _service.GetListForHeader(dto);
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeSignerView)]
        public PagedResult<AppointEmployeeListDto> GetListForSigner([FromBody] AppointEmployeeSortFilterOptions dto)
        {
            return _service.GetListForSigner(dto);
        }
        [Authorize]
        [HttpGet]
        [Authorize(ModuleCode.AppointEmployeeView)]
        [ProducesResponseType(typeof(AppointEmployeeDto), 200)]
        public IActionResult Get()
        {
            return Ok(_service.Get());
        }
        [Authorize]
        [HttpGet("{id}")]
        [Authorize(ModuleCode.AppointEmployeeView, ModuleCode.AppointEmployeeSignerView, ModuleCode.SignerView)]
        [ProducesResponseType(typeof(AppointEmployeeDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeViewAll)]
        [ProducesResponseType(typeof(SelectList<long>), 200)]
        public IActionResult GetAsSelectList(int? employeeId = null)
        {
            return Ok(_service.AsSelectList(employeeId));
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeCreate, ModuleCode.AllAppointEmployeeCreate)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public async ValueTask<IActionResult> Create(CreateAppointEmployeeDlDto dto)
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
        [Authorize]
        [HttpPost]
        public async ValueTask<IActionResult> WebImzoSign(WebImzoSignedFilter filter)
        {
            if (ModelState.IsValid)
            {
                var url = await _service.SendUrl(filter.Id);

                if (_service.IsValid)
                    return Ok(new { Url = url });

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeEdit)]
        [ProducesResponseType(200)]
        public IActionResult Update(UpdateAppointEmployeeDlDto dto)
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
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeAccept)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини тасдиқлаш")]
        public async Task<IActionResult> Accept(UpdateStatusAppointEmployeeDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AcceptAsync(dto);
                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeCancel)]
        [ProducesResponseType(200)]
        //[UserAction("Ходимни тайинлаш ҳужжатини бекор қилиш")]
        public IActionResult Cancel(UpdateStatusAppointEmployeeDto dto)
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
        [Authorize]
        [HttpPost]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public async Task<IActionResult> Sign(SignStatusAppointEmployeeDto dto)
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
        [Authorize]
        [HttpPost("{id}")]
        [Authorize(ModuleCode.AppointEmployeeDelete)]
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

        [Authorize]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult GenerateWord()
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GenerateWord();

                if (_service.IsValid)
                    return File(bytes, "application/msword", fileDownloadName: "AutoGeneratedTemplate.docx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpGet]
        public IActionResult CheckEmploymentRateBeforSave(int employeeId, DateTime startOn, decimal employeeRate, int? fromPositionId)
        {
            if (ModelState.IsValid)
            {
                _service.CheckEmploymentRateBeforSave(employeeId, startOn, employeeRate, fromPositionId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [Authorize]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> Print(long id)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.Print(id);

                if (_service.IsValid)
                    return File(bytes, "application/pdf", fileDownloadName: "AutoGeneratedTemplate.pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
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
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public IActionResult MehnatStart()
        {
            if (ModelState.IsValid)
            {
                _mehnatService.UpdateEmployeesWorkData();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }



        [HttpPost]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public IActionResult WokdActivityStart()
        {
            if (ModelState.IsValid)
            {
                _workActivity.UpdateWorkActivity();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public IActionResult WokdXodimCount()
        {
            if (ModelState.IsValid)
            {
                int count = _workActivity.GetXodimDCount();

                if (_service.IsValid)
                    return Ok(count);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }



        [HttpPost]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPhoto()
        {
            if (ModelState.IsValid)
            {
                await _photoUploader.UploadPhotos();

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public IActionResult CountEmployeeWithoutPhoto()
        {
            if (ModelState.IsValid)
            {
                int count = _photoUploader.NoPhotoCount();

                if (_service.IsValid)
                    return Ok(count);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost("{id}")]
        //[Authorize(ModuleCode.AppointEmployeeSign, ModuleCode.AppointEmployeeWithoutSigner)]
        [AllowAnonymous]
        public async Task<IActionResult> UploadPhotByid(int id)
        {
            if (ModelState.IsValid)
            {
                await _photoUploader.UploadByEmployeeId(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadTemplate()
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.GetWordTemplate();

                if (_service.IsValid)
                    return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DownloadTemp.docx");

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
        [HttpPost]
        [Authorize(ModuleCode.AppointEmployeeSignAnyway)]
        public async Task<IActionResult> SignUpdate(long id)
        {
            if (ModelState.IsValid)
            {
                await _service.SignUpdate(id);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
