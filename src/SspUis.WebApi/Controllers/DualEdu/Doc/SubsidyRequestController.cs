using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("Dual/[controller]/[action]")]
public class SubsidyRequestController : WebaseController
{
    private ISubsidyRequestService _service;

    public SubsidyRequestController(ISubsidyRequestService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.SubsidyRequestView)]
    public PagedResult<SubsidyRequestListDto> GetList([FromBody] SubsidyRequestSortFilterOption dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.SubsidyRequestView)]
    [ProducesResponseType(typeof(SubsidyRequestDto), 200)]
    public IActionResult Get()
    {
        var dto = _service.Get();

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.SubsidyRequestView)]
    [ProducesResponseType(typeof(SubsidyRequestDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.SubsidyRequestReject)]
    [ProducesResponseType(200)]
    public IActionResult Reject(UpdateStatusSubsidyRequestDlDto dto)
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
    [Authorize(ModuleCode.SubsidyRequestAccept)]
    [ProducesResponseType(200)]
    public IActionResult Accept(UpdateStatusSubsidyRequestDlDto dto)
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
    [Authorize(ModuleCode.SubsidyRequestDelete)]
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
    [Authorize(ModuleCode.SubsidyRequestCancel)]
    [ProducesResponseType(200)]
    public IActionResult Cancel(UpdateStatusSubsidyRequestDlDto dto)
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
}
