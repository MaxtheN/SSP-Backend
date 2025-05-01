using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Hrm;

[ApiController]
[Route("hrm/[controller]/[action]")]
public class EmployeeSendTrainController : WebaseController
{
    private readonly IEmployeeSendTrainService _service;

    public EmployeeSendTrainController(IEmployeeSendTrainService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }


    [HttpPost]
    [Authorize(ModuleCode.EmployeeSendTrainViewAll)]
    public PagedResult<EmployeeSendTrainListDto> GetList([FromBody] EmployeeSendTrainSortFilterOptions options)
    {
        return _service.GetList(options);
    }
    [HttpPost]
    [Authorize(ModuleCode.EmployeeSendTrainSignerView)]
    public PagedResult<EmployeeSendTrainListDto> GetListForSigner([FromBody] EmployeeSendTrainSortFilterOptions dto)
    {
        return _service.GetListForSigner(dto);
    }
    //[HttpPost]
    //[Authorize(ModuleCode.EmployeeLeaveOrderView)]
    //public PagedResult<UnpaidEmployeeSendTrainListDto> GetUnpaidList([FromBody] UnpaidEmployeeSendTrainSortFilterPageOptions dto)
    //{
    //    return _service.GetUnPaidList(dto);
    //}
    [HttpGet]
    [Authorize(ModuleCode.EmployeeSendTrainView)]
    [ProducesResponseType(typeof(EmployeeSendTrainDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.EmployeeSendTrainView, ModuleCode.EmployeeSendTrainSignerView, ModuleCode.SignerView)]
    [ProducesResponseType(typeof(EmployeeSendTrainDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(int? employeeId = null)
    {
        return Ok(_service.AsSelectList(employeeId));
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeSendTrainCreate, ModuleCode.AllEmployeeSendTrainCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async ValueTask<IActionResult> Create(CreateEmployeeSendTrainDlDto dto)
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
    [Authorize(ModuleCode.EmployeeSendTrainEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateEmployeeSendTrainDlDto dto)
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
    [Authorize(ModuleCode.EmployeeSendTrainAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Малака оширишга юбориш ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusEmployeeSendTrainDto dTo)
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
    [Authorize(ModuleCode.EmployeeSendTrainSign)]
    public async Task<IActionResult> Sign(SignStatusEmployeeSendTrainDto dto)
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

    [Authorize(ModuleCode.EmployeeSendTrainSign)]
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
    [HttpPost]
    [Authorize(ModuleCode.EmployeeSendTrainCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Малака оширишга юбориш ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusEmployeeSendTrainDto dTo)
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
    [HttpPost("{id}")]
    [Authorize(ModuleCode.EmployeeSendTrainDelete)]
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

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
}
