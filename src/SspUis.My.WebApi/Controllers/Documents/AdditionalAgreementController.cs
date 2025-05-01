using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers.Memship;

[ApiController]
[Authorize]
[Route("Memship/[controller]/[action]")]
public class AdditionalAgreementController : WebaseController
{
    private readonly IAdditionalAgreementService _service;

    public AdditionalAgreementController(IAdditionalAgreementService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AdditionalAgreementDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    public PagedResult<AdditionalAgreementListDto> GetList([FromBody] SortFilterPageOptions options)
    {
        return _service.GetList(options);
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sign(SignStatusAdditionalAgreementDto dto)
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2, lang);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
