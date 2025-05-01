using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Srv
{
    [Authorize]
    [ApiController]
    [Route("srv/[controller]/[action]")]
    public class ServiceApplicationController : WebaseController
    {
        private readonly ISrvApplicationService _service;
        public ServiceApplicationController(ISrvApplicationService service)
             : base(AppSettings.Instance.ControllerConfig)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.SrvServiceApplicationViewAll)]
        public PagedResult<SrvApplicationListDto> GetList([FromBody] SrvApplicationSortFilterOptions dto)
        {
            return _service.GetList(dto);
        }

        [HttpGet]
        [Authorize(ModuleCode.SrvServiceApplicationView)]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult Get()
        {
            SrvApplicationDto? dto = _service.Get();

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpPost("{id}")]
        [Authorize(ModuleCode.SrvServiceApplicationDelete)]
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

        [HttpGet("{id}")]
        [Authorize(ModuleCode.SrvServiceApplicationView)]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult Get(long id)
        {
            var dto = _service.Get(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{id2}")]
        [ProducesResponseType(typeof(SrvApplicationDto), 200)]
        public IActionResult GetById2(Guid id2)
        {
            var dto = _service.Get(id2);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        //[HttpPost]
        //[Authorize(ModuleCode.SrvServiceApplicationReject)]
        //[ProducesResponseType(200)]
        //public IActionResult Reject(RejectStatusSrvApplicationDto dto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _service.Reject(dto);

        //        if (_service.IsValid)
        //            return Ok();

        //        _service.CopyErrorsToModelState(ModelState);
        //    }

        //    return ValidationProblem(ModelState);
        //}

        [HttpPost]
        [Authorize(ModuleCode.SrvServiceApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult AcceptForFree(AcceptStatusSrvApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.AcceptForFree(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [Authorize(ModuleCode.SrvServiceApplicationAccept)]
        [ProducesResponseType(200)]
        public IActionResult Accept(AcceptStatusSrvApplicationDto dto)
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
        [Authorize(ModuleCode.SrvServiceApplicationReceived)]
        [ProducesResponseType(200)]
        public IActionResult Received(RecievedStatusSrvApplicationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.Received(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Authorize(ModuleCode.SrvServiceApplicationCancel)]
        [ProducesResponseType(200)]
        public IActionResult Cancel(CancelStatusSrvApplicationDto dto)
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
        [AllowAnonymous]
        [ProducesResponseType(200)]
        public IActionResult DownloadPdf(Guid id2, string? lang)
        {
            if (ModelState.IsValid)
            {
                var bytes = _service.DownloadPdf(id2, lang);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        public IActionResult PrinSrvApplicationExcel(SrvApplicationSortFilterOptions dto)
        {
            if (ModelState.IsValid)
            {
                var file = _service.PrinSrvApplicationExcel(dto);
                if (_service.IsValid)
                    return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SrvApplicationView.xlsx");

                _service.CopyErrorsToModelState(ModelState);
            }
            return ValidationProblem(ModelState);
        }
    }
}
