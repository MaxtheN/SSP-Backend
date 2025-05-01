using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Memship;
using SspUis.Core.Security;
using WbImzo.Models;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Memship;

[ApiController]
[Route("Memship/[controller]/[action]")]
public class MemshipContractController : WebaseController
{
    private readonly IMemshipContractService _service;

    public MemshipContractController(IMemshipContractService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }
    [Authorize]
    [HttpPost]
    [AllowAnonymous]
    public PagedResult<MemshipContractListDto> GetList([FromBody] MemshipContractSortFilterOptions options)
    {
        return _service.GetList(options);
    }

	[HttpPost]
	[Authorize]
	public IActionResult GetCount()
	{
		int result = _service.GetCount();
		return Ok(result);
	}
	[Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(MemshipContractDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
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
                return Ok(new { status = "success", message = result.Url});

            _service.CopyErrorsToModelState(ModelState);
            
        }

        return ValidationProblem(ModelState);
    }


    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult Sign(SignStatusMemshipContractDto dto)
    {
        if(ModelState.IsValid)
        {
            _service.Sign(dto);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MemshipContractDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
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

            if (_service.IsValid)
                return  File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


    [HttpPost]
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
}