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
public class RecallLeaveController :WebaseController
{
    private readonly IRecallLeaveService _service;

    public RecallLeaveController(IRecallLeaveService service)
        :base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.RecallLeaveViewAll)]
    public PagedResult<RecallLeaveListDto> GetList([FromBody] RecallLeaveSortFilterOptions options)
    {
        return _service.GetList(options);
    }
    [HttpPost]
    [Authorize(ModuleCode.RecallLeaveSignerView)]
    public PagedResult<RecallLeaveListDto> GetListForSigner([FromBody] RecallLeaveSortFilterOptions dto)
    {
        return _service.GetListForSigner(dto);
    }
    [HttpGet]
    [Authorize(ModuleCode.RecallLeaveView)]
    [ProducesResponseType(typeof(RecallLeaveDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.RecallLeaveView, ModuleCode.RecallLeaveSignerView)]
    [ProducesResponseType(typeof(RecallLeaveDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.RecallLeaveViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(RecallLeaveSortFilterOptions options)
    {
        return Ok(_service.AsSelectList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.RecallLeaveCreate, ModuleCode.AllRecallLeaveCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async ValueTask<IActionResult> Create(CreateRecallLeaveDlDto dto)
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
    [Authorize(ModuleCode.RecallLeaveEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateRecallLeaveDlDto dto)
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
    [Authorize(ModuleCode.RecallLeaveAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Малака оширишга юбориш ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusRecallLeaveDto dTo)
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
    [Authorize(ModuleCode.RecallLeaveSign)]
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
    [Authorize(ModuleCode.RecallLeaveSign)]
    public async Task<IActionResult> Sign(SignStatusRecallLeaveDto dto)
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
    [Authorize(ModuleCode.RecallLeaveCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Малака оширишга юбориш ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusRecallLeaveDto dTo)
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
    [Authorize(ModuleCode.RecallLeaveDelete)]
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
    public async Task<IActionResult> DownloadPdf(Guid id2, string lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2 , lang);

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
