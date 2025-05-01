using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.BRBankSerives;
using SspUis.BizLogicLayer.BRBankSerives.Concrete;
using SspUis.DataLayer.Repositories.Appeal;
using WEBASE.AspNet;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BRBankIntegrationController : ControllerBase
    {
        private readonly IBRBankService _service;

        private readonly IMemshipCertificateService _memshipService;



        public BRBankIntegrationController(IBRBankService service, IMemshipCertificateService memshipService)
        {
            _service = service;
            _memshipService = memshipService;
        }

        [HttpPost]
        [IntegrationAuthorize("brbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostAppeal(CreateAppealApplicationDlDto dto)
        {
            if (ModelState.IsValid)
            {
               var result = await _service.GetResult(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("brbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PersonInfo(BrBankPersonDlDto dto)
        {
            if (ModelState.IsValid)
            {
               var result = await _service.GetByPassportDataFromDigital(dto);
                if(result is null)
                    return BadRequest(result);

                if (_service.IsValid)
                     return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [IntegrationAuthorize("brbank")]
        [ProducesResponseType(200)]
        public IActionResult GetByInnPinflForChamber(string innPinfl)
        {
            if (ModelState.IsValid)
            {
                var res = _memshipService.GetByInnPinflForChamber(innPinfl);

                if (_memshipService.IsValid)
                    return Ok(res);

                _memshipService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [IntegrationAuthorize("brbank")]
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
        [IntegrationAuthorize("brbank")]
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

