using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("Memship/[controller]/[action]")]
public class MemshipCertificateController : WebaseController
{
    private readonly IMemshipCertificateService _service;

    public MemshipCertificateController(IMemshipCertificateService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateViewAll)]
    public PagedResult<MemshipCertificateListDto> GetList([FromBody] MemshipCertificateSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.MemshipCertificateView)]
    [ProducesResponseType(typeof(MemshipCertificateDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

	[HttpGet]
	[AllowAnonymous]
	[ProducesResponseType(typeof(long), 200)]
	public IActionResult GetCount()
	{
		return Ok(_service.GetCount());
	}
	[HttpGet]
    //[Authorize(ModuleCode.MemshipCertificateCheckFromSoliq)]
    [ProducesResponseType(typeof(MemshipCertificateFromSoliqDto), 200)]
    public async Task<IActionResult> GetFromSoliq(string inn)
    {
        if (ModelState.IsValid)
        {
            var res = await _service.GetFromSoliq(inn);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpGet("{id}")]
    [Authorize(ModuleCode.MemshipCertificateView)]
    [ProducesResponseType(typeof(MemshipCertificateDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadPdf(Guid id2, string? lang)
    {
        if(ModelState.IsValid)
        {
            var bytes = _service.DownloadPdf(id2, lang);

            if(_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadPdfCopy(MemshipCertificateForPdf dto)
    {
        if (ModelState.IsValid)
        {
            var bytes = _service.DownloadPdf(dto);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpGet("{memshipContractId}")]
    [Authorize(ModuleCode.MemshipCertificateView)]
    [ProducesResponseType(typeof(MemshipCertificateDto), 200)]
    public IActionResult GetByMemshipContractId(long memshipContractId)
    {
        var memshipContract = _service.GetByMemshipContractId(memshipContractId);

        if (_service.IsValid)
            return Ok(memshipContract);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }
    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async Task<IActionResult> Create(CreateMemshipCertificateDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<long> result = await _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMemshipCertificateDlDto dto)
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
	[Authorize(ModuleCode.MemshipCertificateEdit)]
	[ProducesResponseType(200)]
	public IActionResult UpdatingBird(UpdatingBirdMemshipCertificateDlDto dto)
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
    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusMemshipCertificateDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Доимий тўлов турлари умумий ҳужжатини бекор қилиш")]
    public async ValueTask<IActionResult> Cancel(CancelStatusMemshipCertificateDto dTo)
    {
        if (ModelState.IsValid)
        {
           await _service.Cancel(dTo);

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
            StorageFile[] dto = files.Select(x => new StorageFile(x.FileName, x.OpenReadStream())).ToArray();
            var result = _service.UploadFiles(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateProlong)]
    [ProducesResponseType(200)]
    public IActionResult ProlongExpireOn(MemshipCertificateProlongDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.ProlongExpireOn(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    public async Task<IActionResult> SendToStat()
    {
        await _service.SendToStat();
		return Ok();
    }
    [HttpPost("{id}")]
    [Authorize(ModuleCode.MemshipCertificateDelete)]
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
    [AllowAnonymous]
    public async Task<IActionResult> ImportJismoniy(List<ImportJISMemshipCertificateDlDto> listDto)
    {
        if (ModelState.IsValid)
        {
            var res = await _service.ImportJismoniy(listDto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ImportYuridik(List<ImportYurMemshipCertificateDlDto> listDto)
    {
        if (ModelState.IsValid)
        {
            var res = await _service.ImportYuridik(listDto);

            if (_service.IsValid)
                return Ok(res);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    public IActionResult SaveAsExcel(MemshipCertificateSortFilterOptions dto)
    {
        if (ModelState.IsValid)
        {
            var file = _service.SaveAsExcel(dto);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipCertificate.xlsx");

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [Authorize(ModuleCode.MemshipCertificateViewAll)]
    public IActionResult ExpiredPaymentCertificate()
    {
        return Ok(_service.MemshipCertificateExpiredPayment());
    }
    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateProlong)]
    [ProducesResponseType(200)]
    public IActionResult ProlongAosExpireOn() 
    {
        if (ModelState.IsValid)
        {
            _service.ProlongAosExpireOn();

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.MemshipCertificateProlong)]
    [ProducesResponseType(200)]
    public IActionResult ProlongQqsExpireOn()
    {
        if (ModelState.IsValid)
        {
            _service.ProlongQqsExpireOn();

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [Authorize(ModuleCode.MemshipCertificateViewAll)]
    public IActionResult MemshipCertificateToPaidNotification()
    {
        return Ok(_service.MemshipCertificateToPaidNotification());
    }
}