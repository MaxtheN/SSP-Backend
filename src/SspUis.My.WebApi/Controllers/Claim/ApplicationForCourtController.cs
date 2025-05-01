using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Claim;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ApplicationForCourtController : WebaseController
{
    private readonly IApplicationForCourtService _service;

    public ApplicationForCourtController(IApplicationForCourtService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }

    [HttpPost]
    public PagedResult<ApplicationForCourtListDto> GetList([FromBody] ApplicationForCourtSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async ValueTask<IActionResult> DownloadPdf(Guid id2)
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

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApplicationForCourtDto), 200)]
    public IActionResult Get(long id)
    {
        ApplicationForCourtDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        return ValidationProblem(ModelState);
    }

}
