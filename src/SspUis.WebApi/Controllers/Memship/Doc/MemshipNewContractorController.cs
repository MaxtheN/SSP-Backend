using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.Repositories.Memship;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Memship;


[Authorize]
[ApiController]
[Route("memship/[controller]/[action]")]
public class MemshipNewContractorController : WebaseController
{
    private IMemshipNewContractorService _service;

    public MemshipNewContractorController(IMemshipNewContractorService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipNewContractorViewAll)]
    public PagedResult<MemshipNewContractorListDto> GetList([FromBody] MemshipNewContractorSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.MemshipNewContractorView)]
    [ProducesResponseType(typeof(MemshipNewContractorDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }
    [HttpGet]
    [ProducesResponseType(typeof(MemshipNewContractorDto), 200)]
    public IActionResult FillTable(int regionId)
    {
        return Ok(_service.FillTable(regionId));
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.MemshipNewContractorView)]
    [ProducesResponseType(typeof(MemshipNewContractorDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipNewContractorViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipNewContractorCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateMemshipNewContractorDlDto dto)
    {
        if (ModelState.IsValid)
        {
            HaveId<long> result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MemshipNewContractorEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMemshipNewContractorDlDto dto)
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
    [Authorize(ModuleCode.MemshipNewContractorAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни тайинлаш ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusMemshipNewContractorDto dto)
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
    [Authorize(ModuleCode.MemshipNewContractorCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни тайинлаш ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusMemshipNewContractorDto dto)
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
    [HttpPost("{id}")]
    [Authorize(ModuleCode.MemshipNewContractorDelete)]
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
