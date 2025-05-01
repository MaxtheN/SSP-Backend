using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Corruption;
using SspUis.Core.Security;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;


[Authorize]
[ApiController]
[Route("corruption/[controller]/[action]")]
public class JoinAntiCorruptionCertificateController : WebaseController
{

    private readonly IJoinAntiCorruptionCertificateService _service;

    public JoinAntiCorruptionCertificateController(
        IJoinAntiCorruptionCertificateService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.JoinAntiCorruptionCertificateViewAll)]
    public PagedResult<JoinAntiCorruptionCertificateListDto> GetList([FromBody] JoinAntiCorruptionCertificateSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.JoinAntiCorruptionCertificateView)]
    [ProducesResponseType(typeof(JoinAntiCorruptionCertificateDto), 200)]
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
    public async ValueTask<IActionResult> DownloadPdf(Guid id2, string? lang)
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
