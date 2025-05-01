using Microsoft.AspNetCore.Mvc;
using WbAccessControl.Sdk;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers.Integration;

[ApiController]
[Route("wbacIntegration/[action]")]
public class WbacIntegrationController : WebaseController
{
    private readonly IWbacIntegrationService _wbacIntegrationService;

    public WbacIntegrationController(IWbacIntegrationService wbacIntegrationService)
    {
        _wbacIntegrationService = wbacIntegrationService;
    }

    [HttpGet]
    [BasicAuth("wb-access-control")]
    public async Task<IActionResult> GetPersonImage([FromBody] WbacImageRequestDto dto)
    {
        if (ModelState.IsValid)
        {
            var streamAndContent = await _wbacIntegrationService.GetPersonImageAsync(dto)
                .ConfigureAwait(false);

            if (streamAndContent != null && _wbacIntegrationService.IsValid)
            {
                return new FileStreamResult(streamAndContent.Value.Item1,
                    streamAndContent.Value.Item2);
            }
            _wbacIntegrationService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [BasicAuth("wb-access-control")]
    public async Task<IActionResult> GetOrganizations()
    {
        if (ModelState.IsValid)
        {
            var organizations = await _wbacIntegrationService.GetOrganizationsAsync()
                .ConfigureAwait(false);

            if (_wbacIntegrationService.IsValid)
                return Ok(organizations);

            _wbacIntegrationService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [BasicAuth("wb-access-control")]
    public async Task<IActionResult> GetPeople()
    {
        if (ModelState.IsValid)
        {
            var people = await _wbacIntegrationService.GetPeopleAsync()
                .ConfigureAwait(false);

            if (_wbacIntegrationService.IsValid)
                return Ok(people);

            _wbacIntegrationService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [BasicAuth("wb-access-control")]
    public async Task<IActionResult> PostPersonTimeLog([FromBody] WbacPersonLogDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = await _wbacIntegrationService.PostTurnstilePersonTimeLogAsync(dto)
                .ConfigureAwait(false);

            if (_wbacIntegrationService.IsValid)
                return Ok(result);

            _wbacIntegrationService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}
