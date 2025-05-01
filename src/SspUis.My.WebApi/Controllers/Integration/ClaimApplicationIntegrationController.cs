using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Claim;
using SspUis.BizLogicLayer.Claim.ClaimThemeServices;
using SspUis.BizLogicLayer.ClaimApplicationServices;
using SspUis.BizLogicLayer.CurrencyServices;
using SspUis.BizLogicLayer.EnumServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.OrganizationServices;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.My.WebApi.Controllers
{
    [ApiController]
    [Route("api/ExternalClaimApplication/[action]")]
    public class ClaimApplicationIntegrationController :
        WebaseController
    {
        private IClaimApplicationService _service;
        private IManualService _manualservice;
        private readonly IApplicationForCourtService _appForCourtService;

        public ClaimApplicationIntegrationController(
            IClaimApplicationService service, IClaimThemeService themeservice, IOrganizationService organizationService, IManualService manualservice, IApplicationForCourtService applicationForCourt
            )
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _manualservice = manualservice;
            _appForCourtService = applicationForCourt;
        }

        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public IActionResult GetClaimApplicationType([FromServices] IClaimManualService _claimManualService, int? langId)
            => Ok(_claimManualService.ClaimApplicationTypeSelectList(langId));
        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public IActionResult GetClaimTheme([FromServices] IClaimThemeService _claimThemeService, int? langId)
         => Ok(_claimThemeService.AsSelectList(langId));

        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public SelectList<int> ClaimResponsibleType([FromServices] IClaimManualService _claimManualService, int? langId)
            => _claimManualService.ClaimResponsibleTypeSelectList(langId);

        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public SelectList<int, LanguageSelectListDto> GetLanguage([FromServices] IManualService _manualservice)
            => _manualservice.LanguageSelectList();


        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public IActionResult GetCurrency([FromServices] ICurrencyService _currencyService, int? langId)
            => Ok(_currencyService.AsSelectList(langId));

        [HttpGet]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        public IActionResult GetOrganization([FromServices] IOrganizationService _organizationService, int? langId)
          => Ok(_organizationService.AsSelectListOrgForBank(langId));

        [HttpGet("{id}")]
        [IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank" , "turonbank", "brbank")]
        [ProducesResponseType(typeof(ClaimApplicationIntegrationForGetDto), 200)]
        public IActionResult GetForIntegration(long id)
        {
            var dto = _service.GetForIntegration(id);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        //[IntegrationAuthorize("sqbbank", "imanbank", "aloqabank", "asakabank", "xalqbank", "mkbank", "kapitalbank", "kkbbank", "davrbank", "agrobank", "turonbank", "brbank")]
        [ProducesResponseType(200)]
        public async ValueTask<IActionResult> DownloadFileWithQrCode(Guid id)
        {
            if (ModelState.IsValid)
            {
                var bytes = await _appForCourtService.DownloadFileWithQrCode(id);

                if (_service.IsValid)
                    return File(bytes, "application/pdf");

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        /// <summary>
        /// Request body max size 30MB
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [IntegrationAuthorize("sqbbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationSqbBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("agrobank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationAgroBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("imanbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationImanBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("aloqabank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationAloqaBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("asakabank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationAsakaBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("xalqbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationXalqBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("mkbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationMkBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("kapitalbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationKapitalBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("kkbbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationKkbBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("davrbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationDavrBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("turonbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationTuronBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [IntegrationAuthorize("brbank")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostClaimApplicationBrBank(ClaimApplicationIntegrationRequestDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateFromIntegration(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
