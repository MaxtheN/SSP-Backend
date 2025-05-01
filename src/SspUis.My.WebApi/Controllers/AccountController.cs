using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.BizLogicLayer.BusinessmanAccountServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Soliq.Models;
using WEBASE.AspNet;
using WEBASE.Utility;

namespace SspUis.My.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IBusinessmanAccountService _service;
        private readonly IAuthService _authService;
        private readonly IContractorService _contractorService;
        public AccountController(IBusinessmanAccountService service, IAuthService authService, IContractorService contractorService)
        {
            _service = service;
            _authService = authService;
            _contractorService = contractorService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] BusinessmanLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Login(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BusinessmanLoginResultDto), 200)]
        public async Task<IActionResult> LoginByEImzo(LoginByEImzoBusinessmanDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.LoginByEImzo(dto);

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResultDto), 200)]
        public async Task<IActionResult> OneIdLogin(OneIdLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.OneIdLogin(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetChallenge()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetChallenge();

                if (_service.IsValid)
                    return Ok(result);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult IntegrationLogin(IntegrationLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.IntegrationLogin(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> SearchByInnPnfl(string innpnfl)
        {
            if (ModelState.IsValid)
            {
                var result = await _contractorService.SearchByInnPnfl(innpnfl: innpnfl);

                if (_contractorService.IsValid)
                {
                    return Ok(result);
                }

                _contractorService.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignInTwoFactor([FromBody] BusinessmanUserSmsCodeDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SignInTwoFactor(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public IActionResult GetUserInfo()
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetUserInfo();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ContractorInfoDto), 200)]
        public IActionResult GetContractorInfo()
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetContractorInfo();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ContractorDocumentInfo), 200)]
        public IActionResult GetContractorDocumentInfo()
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetContractorDocumentInfo();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ContractorMemshipStateDto), 200)]
        public IActionResult GetContractorMemshipState()
        {
            if (ModelState.IsValid)
            {
                var result = _service.GetContractorMemshipState();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registrate([FromBody] RegistrateBusinessmanDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.Registrate(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult IsUserRegistered([FromBody] IsBusinessmanUserRegisteredDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.IsUserRegistered(dto);

                if (_service.IsValid)
                {
                    return Ok(new { Success = result });
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SendSMSCode([FromBody] BusinessmanUserVerifyCodeDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.SendSMSCode(dto);

                if (_service.IsValid)
                {
                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CheckSMSCode([FromBody] BusinessmanUserSmsCodeDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.IsValidSMSCode(dto);

                if (_service.IsValid)
                {
                    return Ok(new { Success = result });
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult ChangePassword([FromBody] BusinessmanChangePasswordDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.ChangePassword(dto);

                if (_service.IsValid)
                {
                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (ModelState.IsValid)
            {
                await _service.Logout();

                if (_service.IsValid)
                {
                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.RestorePassword(dto);

                if (_service.IsValid)
                {
                    return Ok(new { success = true });
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RestorePasswordConfirm([FromBody] RestorePasswordDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.RestorePasswordConfirm(dto);

                if (_service.IsValid)
                {
                    return Ok(new { success = true });
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetUserLanguage([FromBody] ChangeBusinessmanUserLanguageDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.ChangeLanguage(dto);

                if (_service.IsValid)
                {
                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetContractorsList()
        {
            if (ModelState.IsValid)
            {
                var result = await _service.GetContractorsList();

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> SelectContractor(string inn)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.SelectContractor(inn, null);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpGet("{inn}")]
        [ProducesResponseType(typeof(SoliqContractorByTinDto), 200)]
        public async Task<ActionResult> GetFromSoliq(string inn)
        {
            var dto = await _service.GetFromTax(inn);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet("{pinfl}")]
        [ProducesResponseType(typeof(SoliqContractorByTinDto), 200)]
        public async Task<ActionResult> GetFromSoliqByPinfl(string pinfl)
        {
            var dto = await _service.GetFromTaxByPinfl(pinfl);

            if (_service.IsValid)
                return Ok(dto);

            _service.CopyErrorsToModelState(ModelState);

            return ValidationProblem(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> SyncWithSoliq()
        {
            if (ModelState.IsValid)
            {
                await _service.SyncWithTax();

                if (_service.IsValid)
                {
                    return Ok();
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserInfo(UpdateBusinessmanAccountUserDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.UpdateUserInfo(dto);

                if (_service.IsValid)
                {
                    return Ok(new { success = true });
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> SendSMSCodeForChangePhoneNumber([FromBody] IsBusinessmanUserRegisteredDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.SendSMSCode(dto);

                if (_service.IsValid)
                    return Ok(new { success = true });

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePhoneNumber([FromBody] BusinessmanUserSmsCodeDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.ChangePhoneNumber(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
        [HttpPost]
        [ProducesResponseType(typeof(List<ContractorSettlementAccountDto>), 200)]
        public IActionResult GetContractorSettlementAccountList()
        {
            return Ok(_service.GetContractorSettlementAccountList());
        }

        [HttpGet]
        public async Task<IActionResult> GetHash()
        {
            if (ModelState.IsValid)
            {
                var res = await _service.GetHash();

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> IsOffer(OfferDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.IsOffer(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetOrganizations(int businessmanUserId)
        {
            if (ModelState.IsValid)
            {
                var res = _service.GetContractorListByUserId(businessmanUserId);

                if (_service.IsValid)
                    return Ok(res);

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetOrganization(SetContractorDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.SetContractorToAuth(dto);

                if (_service.IsValid)
                {
                    return Ok(result);
                }

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewOrganization(ToAddOrganizationDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddNewContractor(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult DeactivateAssociation(DeactivateAssociationDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.DeactivateAssociation(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult UpdateSettlementAccount(UpdateContractorSettlementAccountDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _contractorService.UpdateSettlementAccount(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult ChangeMainSettlementAccount(long contractorId, long settlementAccountId)
        {
            if (ModelState.IsValid)
            {
                _contractorService.ChangeBasicSettlementAccounting(contractorId, settlementAccountId);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
