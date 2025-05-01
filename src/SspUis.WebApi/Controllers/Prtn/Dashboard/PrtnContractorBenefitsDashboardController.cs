using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Partner;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers.Prtn.Dashboard
{
    [ApiController]
    [Authorize]
    [Route("prtn/[controller]/[action]")]
    public class PrtnContractorBenefitsDashboardController : WebaseController
    {
        private readonly IPrtnReportsDashboardService _service;
        public PrtnContractorBenefitsDashboardController(IPrtnReportsDashboardService service) => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public async Task<IActionResult> GetPrtnReportsStatisticsList(DashboardFilterOption options)
        {
            return Ok(await _service.GetPrtnStatisticsList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnReportsCustomsPrivilegeList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnCustomsPrivilegeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnReportsEntrepreneurFoundationList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnEntrepreneurFoundationList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnReportsCreditsList(DashboardFilterOption options)
        {
            return Ok(_service.GetReportsCreditsList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnReportsStateAssetsList(DashboardFilterOption options)
        {
            return Ok(_service.GetReportsStateAssetsList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnRepotsTaxCreditsList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnTaxPrivilegeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.DashboardView)]
        public IActionResult GetPrtnReportsRateList(RegionRateFilterOption options)
        {
            return Ok(_service.GetPrtnRateList(options));
        }
    }
}
