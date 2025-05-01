using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ArbitrationCourtApplicationServices;
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
    public class ArbitrationCourtApplicationController : WebaseController
    {
        private IArbitrationCourtApplicationService _service;

        public ArbitrationCourtApplicationController(IArbitrationCourtApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationCourtApplicationView)]
        [ProducesResponseType(typeof(PagedResult<ArbitrationCourtApplicationListDto>), 200)]

        public IActionResult GetList([FromBody] ArbitrationCourtApplicationSortFilterOptions dto)
        {
            var res = _service.GetList(dto);
            if (_service.IsValid)
                return Ok(res);


            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ArbitrationCourtApplicationView)]
        [ProducesResponseType(typeof(ArbitrationCourtApplicationDto), 200)]
        public async Task<IActionResult> Get()
        {
            var dto = await _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [Authorize(ModuleCode.ArbitrationCourtApplicationView)]
        [ProducesResponseType(typeof(ArbitrationCourtApplicationDto), 200)]
        public IActionResult GetByContractorId(long contractorId)
        {
            ArbitrationCourtApplicationDto dto = _service.GetByContractorId(contractorId);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ArbitrationCourtApplicationView)]
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
        [Authorize(ModuleCode.ArbitrationCourtApplicationDelete)]
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
        [Authorize(ModuleCode.ArbitrationCourtApplicationReject)]
        [ProducesResponseType(200)]
        public IActionResult Reject(RejectStatusArbitrationCourtApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Reject(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        /// <summary>
        /// DiscussionDate: 4-stepda kiritish kerak bo'ladigan polya erp da chiqadi kiritish joyi(kalendar).
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(ModuleCode.ArbitrationCourtApplicationCreate)]
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
        [Authorize(ModuleCode.ArbitrationCourtApplicationEdit)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult NextStep(long id, bool? withoutDelay)
        {
            if (ModelState.IsValid)
            {
                HaveId<long> res = _service.NextStep(id);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.ArbitrationCourtApplicationEdit)]
        [ProducesResponseType(typeof(HaveId<long>), 200)]
        public IActionResult Update(UpdateArbitrationCourtApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
                var res = _service.Update(dto);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Authorize(ModuleCode.ArbitrationCourtApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusArbitrationCourtApplicationDto dto)
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
        [Authorize(ModuleCode.ArbitrationCourtApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusArbitrationCourtApplicationDto dTo)
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
        [Authorize(ModuleCode.ChastisementSign)]
        public async Task<IActionResult> Sign(SignStatusArbitrationCourtApplicationDto dto)
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
        [Authorize(ModuleCode.ArbitrationCourtApplicationDelete)]
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
        public async Task<IActionResult> DownloadTemplate(string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _service.DownloadTemplate(lang);

                if (_service.IsValid)
                    return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DownloadTemp.docx");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
