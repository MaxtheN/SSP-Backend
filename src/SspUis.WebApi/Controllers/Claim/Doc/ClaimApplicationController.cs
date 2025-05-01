using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ClaimApplicationServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClaimApplicationController : WebaseController
    {
        private IErpClaimApplicationService _service;

        public ClaimApplicationController(IErpClaimApplicationService service)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(DocNumbersByClaimAppTypeDto), 200)]
        public IActionResult GetListDocNumbersByClaimAppTypeId(ByClaimAppTypeFilter dto)
        {
            return Ok(_service.GetDataDocNumbersByClaimAppTypeId(dto));
        }

        [HttpPost]
        [Authorize(ModuleCode.ClaimApplicationView)]
        public PagedResult<ClaimApplicationListDto> GetList([FromBody] ClaimApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.ClaimApplicationView)]
        [ProducesResponseType(typeof(ClaimApplicationDto), 200)]
        public IActionResult Get()
        {
            var dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id}")]
        [Authorize(ModuleCode.ClaimApplicationView)]
        [ProducesResponseType(typeof(ClaimApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            ClaimApplicationDto? dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
		
		[HttpGet("{id}")]
		[Authorize(ModuleCode.ClaimApplicationView)]
		[ProducesResponseType(typeof(ClaimApplicationDto), 200)]
		public IActionResult GetForInfo(long id, int stepId)
		{
			var dto = _service.GetForInfo(id, stepId);

			if (_service.IsValid)
				return Ok(dto);

			_service.CopyErrorsToModelState(ModelState);

			return ValidationProblem(ModelState);
		}

		[HttpPost]
        [Authorize(ModuleCode.ClaimApplicationReject)]
        [ProducesResponseType(200)]
        public IActionResult Reject(RejectStatusClaimApplicationDto dto)
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

        [HttpPost]
        [Authorize(ModuleCode.ClaimApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusClaimApplicationDto dto)
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
        [Authorize(ModuleCode.ClaimApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusClaimApplicationDto dto)
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

        [HttpPost]
        [Authorize(ModuleCode.ClaimApplicationEdit)]
        [ProducesResponseType(200)]
        public IActionResult EmployeeAttachment(UpdateEmployeeAttechmentDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.EmployeeAttachment(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadPdf(Guid id2, string? lang = null)
        {
            if (ModelState.IsValid)
            {
                var bytes =await _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

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

        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult UpdateClaimApplicationTable(long id, string newAddress)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateClaimApplicationTable(id, newAddress);
                if (_service.IsValid)
                    return Ok();
                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }

    }
}
