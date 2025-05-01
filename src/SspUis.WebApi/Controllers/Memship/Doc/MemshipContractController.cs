using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.OfficeTools.Extensions;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Memship;

[ApiController]
[Route("Memship/[controller]/[action]")]
public class MemshipContractController : WebaseController
{
    private readonly IMemshipContractService _service;

    public MemshipContractController(
        IMemshipContractService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipContractViewAll)]
    public PagedResult<MemshipContractListDto> GetList([FromBody] MemshipContractSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    [Authorize(ModuleCode.MemshipContractView)]
    [ProducesResponseType(typeof(MemshipContractDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.MemshipContractView)]
    [ProducesResponseType(typeof(MemshipContractDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpGet("{applicationId}")]
    [Authorize(ModuleCode.MemshipContractView)]
    [ProducesResponseType(typeof(MemshipContractDto), 200)]
    public IActionResult GetByApplicationId(int applicationId)
    {
        var dto = _service.GetByApplicationId(applicationId);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipContractCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async ValueTask<IActionResult> Create(CreateMemshipContractDlDto dto)
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
    [Authorize(ModuleCode.MemshipContractEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMemshipContractDlDto dto)
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
	[HttpPost]
	[Authorize(ModuleCode.MemshipContractEdit)]
	[ProducesResponseType(200)]
	public IActionResult UpdatingBird(UpdatingBirdMemshipContractDlDto dto)
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

	[HttpGet]
    [Authorize(ModuleCode.MemshipContractEdit)]
    [ProducesResponseType(200)]
    public IActionResult Comfirm(long id)
    {
        if (ModelState.IsValid)
        {
            _service.Comfirm(id);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipContractCancel)]
    [ProducesResponseType(200)]
    public async ValueTask<IActionResult> Cancel(CancelStatusMemshipContractDto dto)
    {
        if (ModelState.IsValid)
        {
           await _service.Cancel(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadFile(Guid? id2, Guid? fileId)
    {
        if (ModelState.IsValid)
        {
            //var bytes = _service.DownloadFile(id2, fileId);

            //if (_service.IsValid)
            //    return File(bytes, "application/pdf");
            return Ok();
            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [Authorize]
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
    [Authorize]
    [HttpGet("{fileId}")]
    [AllowAnonymous]
    [ProducesResponseType( 200)]
    public IActionResult DownloadByIdFile(Guid fileId)
    {
        if (ModelState.IsValid)
        {
            StorageFile file = _service.DownloadByIdFile(fileId);

            if (_service.IsValid)
                return File(file.GetStream(), "application/pdf");

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
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadPdfCopy(MemshipContractDto model)
    {
        if (ModelState.IsValid)
        {
            var bytes = _service.DownloadPdf(model);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipContractSign)]
    [ProducesResponseType(200)]
    public IActionResult Sign(SignStatusMemshipContractDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Sign(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public async ValueTask<IActionResult> SignWebImzo(SignWebImzoContractFilter filter)
    {
        if (ModelState.IsValid)
        {
            (string? Url, bool Result) result = await _service.WebImzoSign(filter);

            if (_service.IsValid && result.Result)
                return Ok(new { status = "success", message = result.Url, result = result.Result});

            _service.CopyErrorsToModelState(ModelState);

           // return Ok(new { status = "error", message = result.Url, result = result.Result });
        }

        return ValidationProblem(ModelState);
    }

    //[HttpPost]
    //[Authorize(ModuleCode.MemshipContractReject)]
    //[ProducesResponseType(200)]
    //public IActionResult Reject(RejectStatusMemshipContractDto dto)
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

    [HttpPost("{id}")]
    [Authorize(ModuleCode.MemshipContractDelete)]
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
    [Authorize]
    public async Task<IActionResult> Import(List<ImportMemshipContractDlDto> listDto)
    {
        if (ModelState.IsValid)
        {
            await _service.Import(listDto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.ChangeContractorToPad)]
    [ProducesResponseType(200)]
    public IActionResult ChangeContractorToPaid(ChangeContractToPayedDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.ChangeContractorToPaid(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.ChangeContractorParametirs)]
    [ProducesResponseType(200)]
    public IActionResult ChangeContractorDocnumber(ChangeContractParametirsDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.ChangeContractorDocnumber(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    #region Jismoniy shaxslarni import qilish uchun xizmat qiladigan. API
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ImportJismoniy(List<ImportJismoniyMemshipContractDlDto> listDto)
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
    public async Task<IActionResult> ImportYuridik(List<ImportYuridikMemshipContractDlDto> listDto)
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
    #endregion

    [HttpPost]
    public IActionResult SaveAsExcel(MemshipContractSortFilterOptions dto)
    {
        if (ModelState.IsValid)
        {
            var file = _service.SaveAsExcel(dto);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemshipContract.xlsx");

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadTemplate(Guid id2)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.GetWordTemplate(id2);

            if (_service.IsValid)
                return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DownloadTemp.docx");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
