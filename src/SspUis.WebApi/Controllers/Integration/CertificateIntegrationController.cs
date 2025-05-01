using Microsoft.AspNetCore.Mvc;
using WEBASE.AspNet;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using WEBASE.AspNet.Security;
using WEBASE.Models;
using SspUis.BizLogicLayer.DistrictServices;
using SspUis.BizLogicLayer.RegionServices;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Route("certificateIntegration/[action]")]
    public class CertificateIntegrationController : WebaseController
    {
        private IDistrictService _districtService;
        private IRegionService _regionService;
        private IPrtnCertificateService _service;

        public CertificateIntegrationController(
            IPrtnCertificateService service,
            IDistrictService districtService,
            IRegionService regionService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _districtService = districtService;
            _regionService = regionService;
        }
        [BasicAuth("certificate")]
        [HttpGet]
        public IActionResult GetCertificateInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if(_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);
            
            return ValidationProblem(ModelState);
        }

        [BasicAuth("certificate")]
        [HttpGet]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectListRegion()
        {
            return Ok(_regionService.AsSelectList());
        }

        [BasicAuth("certificate")]
        [HttpGet()]
        [ProducesResponseType(typeof(SelectList<int>), 200)]
        public IActionResult GetAsSelectListDistrict(int? regionId)
        {
            return Ok(_districtService.AsSelectList(regionId));
        }


        [BasicAuth("mahalla_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForMahallaInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [BasicAuth("moliya_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForMoliyaInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [BasicAuth("asakabank_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForAsakabankInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [BasicAuth("xalq_bank_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForXalqBankInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [BasicAuth("bandlik_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForBandlikInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [BasicAuth("davrbank_certificate")]
        [HttpGet]
        public IActionResult GetCertificateForDavrbankInfo(int? lang, Guid? Id2, string? contractorInn)
        {
            var dto = _service.GetCertificateInfo(lang ?? 1, Id2, contractorInn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }
    }
}
