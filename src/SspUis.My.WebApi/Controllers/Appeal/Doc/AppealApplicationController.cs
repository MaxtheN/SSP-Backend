using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Appeal;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.Srv;
using SspUis.DataLayer.Repositories.Appeal;
using WbImzo.Models;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers;

[ApiController]
[Route("appeal/[controller]/[action]")]
public class AppealApplicationController : WebaseController
{
    private IAppealApplicationService _service;

    public AppealApplicationController(IAppealApplicationService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    public PagedResult<AppealApplicationListDto> GetList([FromBody] AppealApplicationSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }
    [HttpGet]
    [ProducesResponseType(typeof(AppealApplicationDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppealApplicationDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

	[HttpPost]
	[Authorize]
	public IActionResult GetCount()
	{
		var result = _service.GetCountMy();
		return Ok(result);
	}

	[HttpPost]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async Task<IActionResult> Create(CreateAppealApplicationDlDto dto)
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
    [HttpPost("{id}")]
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
    [ProducesResponseType(200)]
    public async Task<IActionResult> Update(UpdateAppealApplicationDlDto dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Update(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public async ValueTask<IActionResult> SignWebImzo(WebImzoSignedFilter filter)
    {
        if (ModelState.IsValid)
        {
            string? result = await _service.WebImzoSign(filter);

            if (_service.IsValid)
                return Ok(new { status = "success", message = result });

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


    [HttpPost]
    public async Task<IActionResult> Sign(SignStatusAppealApplicationDto dto)
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