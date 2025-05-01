using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.DualContractServices;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Configurations;
using SspUis.Integration.Billing.Services;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers;

[ApiController]
[Route("Dual/[controller]/[action]")]
public class DualContractController : WebaseController
{
    private readonly IDualContractService _service;
    private readonly SystemConf _systemConf;
    private readonly IBillingService _billingService;

    public DualContractController(IDualContractService service, SystemConf systemConf, IBillingService billingService)
    {
        _service = service;
        _systemConf = systemConf;
        _billingService = billingService;
    }
    [Authorize]
    [HttpPost]
    public PagedResult<DualContractListDto> GetList([FromBody] DualContractDtoSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DualContractDto), 200)]
    public IActionResult Get(int id)
    {
        DualContractDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

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
    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sign(UpdateDualContract dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Sign(dto);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


	[HttpPost]
	public IActionResult GetCount()
	{
		var result = _service.GetCount();
		return Ok(result);
	}
	/// <summary>
	/// Bu keyinchalik kerak bo'lishi mumkin...
	/// </summary>
	/// <param name="id2"></param>
	/// <param name="lang"></param>
	/// <returns></returns>
	//[HttpPost]
	//[ProducesResponseType(200)]
	//public IActionResult Reject(RejectStatusDualContractDto dto)
	//{
	//    if (ModelState.IsValid)
	//    {
	//        _service.Reject(dto);

	//        if(_service.IsValid)
	//            return Ok();

	//        _service.CopyErrorsToModelState(ModelState);
	//    }

	//    return ValidationProblem(ModelState);
	//}

	[HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2, lang);

            if (_service.IsValid)
                return File(bytes, "application/pdf", fileDownloadName: "DualContract.pdf");

            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}