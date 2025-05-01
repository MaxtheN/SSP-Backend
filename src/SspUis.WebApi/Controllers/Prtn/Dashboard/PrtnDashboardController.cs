using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Partner;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("prtn/dashboard/[controller]/[action]")]
    public class PrtnDashboardController : WebaseController
    {
        private readonly IDashboardService _service;
        public PrtnDashboardController(IDashboardService service)
            => _service = service;

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnDocumentsRegionRate(RegionRateFilterOption options)
        {
            return Ok(_service.GetPrtnDocumentsRegionRate(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnCertificate(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnCertificateList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnContractsList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnContractList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnApplicationList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnApplicationList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnStatisticsList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnStatisticsList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnContractTypeList(DashboardFilterOption options)
        {
            return Ok(_service.GetPrtnContractTypeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.PartnerDashboardView)]
        public IActionResult GetPrtnContractsRegionRate(RegionRateFilterOption options)
        {
            return Ok(_service.GetPrtnContractsRegionRate(options));
        }
    }
}
