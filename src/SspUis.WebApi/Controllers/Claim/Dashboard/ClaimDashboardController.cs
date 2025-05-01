using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Claim;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("ClaimDashboard/[action]")]
public class ClaimDashboardController : WebaseController
{
    private readonly IClaimDashboardService _service;
    public ClaimDashboardController(IClaimDashboardService service) 
        => _service = service;

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetClaimStatistic(ClaimDashFilterOption options)
    {
        return Ok(_service.GetClaimStatisticList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetClaimApplicationList(ClaimDashFilterOption options)
    {
        return Ok(_service.GetClaimApplicationList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetCourtApplicationList(ClaimDashFilterOption options)
    {
        return Ok(_service.GetApplicationForCourtList(options));    
    }

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetMediationResultList(ClaimDashFilterOption options)
    {
        return Ok(_service.GetMediationResultList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetClaimApplicationRateList(ClaimDashRateFilterOption options)
    {
        return Ok(_service.GetApplicationRateList(options));
    }

    [HttpPost]
    [Authorize(ModuleCode.ClaimDashboardView)]
    public IActionResult GetContractorsRateList(ClaimContractorTimeLineOption options)
    {
        return Ok(_service.GetContractorsRateList(options));
    }
}
