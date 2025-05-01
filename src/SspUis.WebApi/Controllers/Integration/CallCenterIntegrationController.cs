using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Administration.ContractorServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.IntegrationServices;
using SspUis.Core.Security;
using SspUis.Integration.Finance.Services;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Controllers.Integration;
[ApiController]
[Route("callcenterIntegration/[action]")]
public class CallCenterIntegrationController : WebaseController
{
    private readonly ICallCenterIntegrationService _service;
    

    public CallCenterIntegrationController(ICallCenterIntegrationService service)
    {
        _service = service;
        
    }
    [BasicAuth("call_center")]
    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult SaveCallCenterCount(int callCount)
    {
        if (ModelState.IsValid)
        {
            var result = _service.CreateCallCenter(callCount);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult GetLastCallCenterCount()
    {
        if (ModelState.IsValid)
        {
            var result = _service.GetLastTime();

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
	[AllowAnonymous]
	[HttpGet]
	[ProducesResponseType(typeof(long), 200)]
	public IActionResult GetCount()
	{
		if (ModelState.IsValid)
		{
			var result = _service.GetLastTime();

			if (_service.IsValid)
				return Ok(result);

			_service.CopyErrorsToModelState(ModelState);
		}

		return ValidationProblem(ModelState);
	}
}
