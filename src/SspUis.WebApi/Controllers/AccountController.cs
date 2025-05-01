using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.DataLayer.Repositories;

namespace SspUis.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("account/[action]")]
    public class AccountController : WebaseController
    {
        private IAccountService _service;
        private readonly IAuthService _authService;

        public AccountController(IAccountService service, IAuthService authService)
            : base(AppSettings.Instance.ControllerConfig)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResultDto), 200)]
        public IActionResult Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = _service.Login(dto);

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

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResultDto), 200)]
        public async Task<IActionResult> LoginByEImzo(LoginByEImzoDto dto)
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

        [HttpGet]
        [ProducesResponseType(typeof(AccountUserDto), 200)]
        public IActionResult GetUserInfo()
        {
            return Ok(_service.GetUserInfo());
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.ChangePassword(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        public IActionResult ChangeLanguage(ChangeUserLanguageDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.ChangeLanguage(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.RecoverPassword(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult RecoveredPassword(RestoredPasswordDlDto dto)
        {
            if (ModelState.IsValid)
            {
                _service.RecoveredPasswordConfirm(dto);

                if (_service.IsValid)
                    return Ok();

                _service.CopyErrorsToModelState(ModelState);
            }

            return ValidationProblem(ModelState);
        }
    }
}
