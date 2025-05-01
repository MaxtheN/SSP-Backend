using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Partner;
using SspUis.BizLogicLayer.Srv.Doc;
using SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.DashboardService;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers.Srv;

[Authorize]
[ApiController]
[Route("srv/[controller]/[action]")]
public class ServiceContractDashboardController : WebaseController
{
    private ISrvApplicationDashService _service;

    public ServiceContractDashboardController(ISrvApplicationDashService service)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    public IActionResult GetSrvApplicationTypeCount(SrvDashboardFilterOption filter)
        => Ok( _service.GetSrvApplicationTypeDash(filter));
    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    [ProducesResponseType(typeof(SrvContractTypeDashDto), 200)]
    public IActionResult GetSrvContractTypeCount(SrvContractTypeDashFilter filter)
        => Ok(_service.GetSrvContractTypeCount(filter));

    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    [ProducesResponseType(typeof(SrvApplicationTypeRateDto), 200)]
    public IActionResult GetSrvApplicationRate(SrvApplicationTypeCostFilter filter)
        => Ok(_service.GetSrvApplicationRate(filter));
    
    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    public IActionResult GetSrvApplicationStatusCount(SrvDashboardFilterOption filter)
        => Ok(_service.GetSrvApplicationStatusCount(filter));

    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    public IActionResult GetSrvContractStatusCount(SrvDashboardFilterOption filter)
        => Ok(_service.GetSrvContractStatusCount(filter));

    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    public IActionResult GetSrvDeedSum(SrvDashboardFilterOption filter)
        => Ok(_service.GetSrvDeedSum(filter));

    [HttpPost]
    [Authorize(ModuleCode.ServiceContractDashboardView)]
    public IActionResult GetSrvDocumentsRegionRate(SrvRegionRateFilterOptions filter)
        => Ok(_service.GetSrvDocumentsRegionRate(filter));
}   