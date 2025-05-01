using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.Extensions;
using System.Security.Claims;
using SspUis.Core.Configurations;
using SspUis.Core.Extensions;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.OneId;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet.Security;
using WEBASE.Integration.EImzo;
using WEBASE.Notify.Sms;

namespace SspUis.BizLogicLayer.AccountServices
{
    public class AccountService : StatusGenericHandler, IAccountService
    {
        private readonly IAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly SystemConf _systemConf;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEImzoService _eImzoService;
        private readonly HttpContext _httpContext;
        private readonly CookieConfig _cookieConfig;
        private readonly SmsProviderConfig _smsProviderConfig;
        private readonly ISmsService _smsService;
        private readonly IOneIdService _oneIdService;

        public AccountService(IUnitOfWork unitOfWork,
                              IAuthService authService,
                              SystemConf systemConf,
                              IHttpContextAccessor httpContextAccessor,
                              IEImzoService eImzoService,
                              CookieConfig cookieConfig,
                              SmsProviderConfig smsProviderConfig,
                              ISmsService smsService,
                              IOneIdService oneIdService)
        {
            _repository = unitOfWork.AccountRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _systemConf = systemConf;
            _httpContextAccessor = httpContextAccessor;
            _eImzoService = eImzoService;
            _httpContext = httpContextAccessor.HttpContext;
            _cookieConfig = cookieConfig;
            _smsProviderConfig = smsProviderConfig;
            _smsService = smsService;
            _oneIdService = oneIdService;
        }

        public AccountUserDto GetUserInfo()
        {
            //var dto = _repository.ById<AccountUserDto>(_authService.User.Id);
            //if (_unitOfWork.Context.Set<UserRole>()
            //                   .IsActive()
            //                   .Include(a => a.Role)
            //                   .Any(a => a.UserId == dto.Id && a.Role.IsAdmin))
            //{
            //    dto.Modules = _unitOfWork.Context.Set<Module>().Select(a => a.Code).ToList();
            //}
            //return dto;
            var dto = _repository.ById<AccountUserDto>(_authService.User.Id);

            if (_unitOfWork.Context.Set<UserRole>()
                                   .IsActive()
                                   .Include(a => a.Role)
                                   .Any(a => a.UserId == dto.Id && a.Role.IsAdmin))
            {
                dto.Modules = _unitOfWork.Context.Set<Module>().Select(a => a.Code).ToList();
            }
            var chastisement = _unitOfWork.Context.Set<ChastisementTable>()
                                          .Any(x =>
                                              x.EmployeeManageId == dto.EmployeeManageId &&
                                              x.IsBlocked);

            if (chastisement)
            {
                dto.Modules = new List<string>
                {
                       ModuleCode.ChastisementSign.ToString(),
                       ModuleCode.ChastisementSignerView.ToString()
                };
            }

            return dto;
        }
        public LoginResultDto Login(LoginDto dto)
        {
            if (dto.UserName.NullOrWhiteSpace() || dto.Password.NullOrWhiteSpace())
            {
                AddError("Имя пользователя или пароль неправильно" , "Message");
                return null;
            }

            var user = _repository.AllAsQueryable//.Include(a => a.Employee)
                                                 .Include(a => a.UserRoles)
                                                 .ThenInclude(a => a.Role)
                                                 .ByUserName(dto.UserName)
                                                 .FirstOrDefault();

            if (user == null || !user.IsValidPassword(dto.Password))
            {
                AddError("Имя пользователя или пароль неправильно", "Message");

                if (user != null)
                    AddUserLog(UserLogAction.IncorrectPasswordEntered, user.UserName, user.Id);
                return null;
            }
            //var subdomain = _httpContextAccessor.HttpContext.Request.Host.Value.Split('.')[0]
            if (_systemConf.CheckForDomain &&
                !_unitOfWork.Context.Set<UserRole>()
                           .IsActive()
                           .Include(a => a.Role)
                           .Any(a => a.UserId == user.Id && a.Role.IsAdmin))
            {
                var userModules = _unitOfWork.Context.Set<UserRole>()
                                                     .Include(a => a.Role)
                                                     .ThenInclude(a => a.RoleModules)
                                                     .ThenInclude(a => a.Module)
                                                     .Where(a => a.UserId == user.Id
                                                              && a.StateId == StateIdConst.ACTIVE)
                                                     .SelectMany(a => a.Role.RoleModules)
                                                     .Select(a => a.Module.Code);
            }

            try
            {
                if (dto.LanguageId.HasValue)
                {
                    _repository.ChangeLanguage(user.Id, new ChangeUserLanguageDlDto
                    {
                        LanguageId = dto.LanguageId.Value
                    });
                }
            }
            catch { }

            if (IsValid)
            {
                AddUserLog(UserLogAction.LoginByPassword, user.UserName, user.Id);
                _repository.UpdateUserLastAccessTime(user.Id);
                string token = _authService.GenerateToken(user.UserName);
                _authService.ResetUserName(user.UserName);

                return new LoginResultDto
                {
                    User = GetUserInfo(),
                    Token = token
                };
            }

            return null;
        }
      
        public async Task<EImzoChallangeResultDto> GetChallenge()
        {
            var challenge = await _eImzoService.Challenge();
            CombineStatuses(_eImzoService);
            return challenge;
        }
        public async Task<LoginResultDto> LoginByEImzo(LoginByEImzoDto dto)
        {
            var res = await _eImzoService.Auth(dto);
            CombineStatuses(_eImzoService);
            if (HasErrors)
                return null;

            if (res?.SubjectCertificateInfo?.SubjectName?.Pinfl == null)
            {
                AddError("Kalit jismoniy shaxsga tegishli emas / Ключ не принадлежит физическому лицу", "Message");
                return null;
            }
            var user = _repository.AllAsQueryable.ByPinfl(res.SubjectCertificateInfo.SubjectName.Pinfl).FirstOrDefault();
            if (user == null)
            {
                AddError("Foydalanuvchi topilmadi / Пользователь не найден", "Message");
                return null;
            }
            try
            {
                if (dto.LanguageId.HasValue)
                {
                    _repository.ChangeLanguage(user.Id, new ChangeUserLanguageDlDto
                    {
                        LanguageId = dto.LanguageId.Value
                    });
                }
            }
            catch { }
            if (IsValid)
            {
                AddUserLog(UserLogAction.LoginByEImzo, user.UserName, user.Id);
                _repository.UpdateUserLastAccessTime(user.Id);
                string token = _authService.GenerateToken(user.UserName);
                _authService.ResetUserName(user.UserName);

                return new LoginResultDto
                {
                    User = GetUserInfo(),
                    Token = token
                };
            }

            return null;
        }
        public async Task<LoginResultDto> OneIdLogin(OneIdLoginDto dto)
        {
            try
            {
                if (dto.Code.NullOrWhiteSpace())
                {
                    AddError("Siz my.chamber tizimida ro'yxatdan o'tmagansiz, " +
                             "ONEID bilan kirish uchun telefon raqam orqali ro'yxatdan o'tishingiz kerak!");
                    return null;
                }
                var accessToken = await _oneIdService.GetAccessToken(dto.Code, dto.RedirectUrl);
                CombineStatuses(_oneIdService);
                if (HasErrors)
                    return null;

                var userData = await _oneIdService.GetUserData(accessToken.AccessToken);
                CombineStatuses(_oneIdService);
                if (HasErrors)
                    return null;
                var user = _repository.AllAsQueryable.ByPinfl(userData.Pin).FirstOrDefault();
                CombineStatuses(_unitOfWork.UserRepository);

                if (IsValid && user != null)
                {
                    AddUserLog(UserLogAction.LoginByOneId, user.UserName ?? "User not found", user.Id);
                    _repository.UpdateUserLastAccessTime(user.Id);
                    
                    string token = _authService.GenerateToken(user.UserName);
                    _authService.ResetUserName(user.UserName);

                    return new LoginResultDto
                    { 
                        User = GetUserInfo(),
                        Token = token
                    };
                }
                await _oneIdService.Logout(accessToken.AccessToken);

                return null;
            }
            catch (Exception ex)
            {
                AddError($"{ex.Message}", "Message");
                throw;
            }            
        }
        public async Task Logout()
        {
            if (!_authService.OneIdToken.NullOrEmpty())
                await _oneIdService.Logout(_authService.OneIdToken);
        }
        public void ChangePassword(ChangePasswordDlDto dto)
        {
            _repository.ChangePassword(_authService.User.Id, dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        public void ChangeLanguage(ChangeUserLanguageDlDto dto)
        {
            _repository.ChangeLanguage(_authService.User.Id, dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        private void AddUserLog(UserLogAction action, string userName, int? userId)
        {
            _repository.AddUserLog(action, userName, userId, _authService.UserIp, _authService.UserAgent);
            _unitOfWork.Save();
        }
        public async Task RecoverPassword(RecoverPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
            {
                AddError("Неправильный номер телефона !", "Message");
                return;
            }

            var user = _repository.ByUserName(dto.Username);
            if (user is null)
            {
                CombineStatuses(_repository);
                return;
            }

            var userIp = _httpContext.GetUserIP();
            var userAgent = string.Empty;

            if (_httpContext.Request.Headers.ContainsKey("User-Agent"))
                userAgent = _httpContext.Request.Headers["User-Agent"];

            var uniqueKey = Core.Extensions.Helper.RndUniqueKey(Core.Extensions.Helper.DEFAULT_LENGTH);
            _httpContext.Response.Cookies.Append($"login-key", uniqueKey, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddMinutes(20)
            });

            string message = _smsProviderConfig.MessageTemplate.FirstOrDefault(x => x.Action == "onlinequeue")?.Text;

            if (string.IsNullOrEmpty(message))
                message = string.Empty;

            Random rnd = new();
            string verifyCode = rnd.Next(1100, 9990).ToString();
            message = string.Format(message, user.UserName, verifyCode);

            if (string.IsNullOrEmpty(message))
                message = verifyCode;

            if (!dto.AppKeyHash.NullOrEmpty())
                message = $"{message} {dto.AppKeyHash}";

            try { user.PhoneNumber = user.PhoneNumber.CorrectPhoneNumber(); }
            catch (Exception ex) { AddError(ex.Message); return; }

            await Task.Run(() => _smsService.Send(user.PhoneNumber, verifyCode));

            _repository.CreateUserDeviceLog(dto.Username, uniqueKey, verifyCode, userIp, userAgent);
            AddUserLog(UserLogAction.RestorePassword, user.PhoneNumber, user.Id);
        }
        public void RecoveredPasswordConfirm(RestoredPasswordDlDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewPassword)
                || dto.NewPassword != dto.ConfirmedNewPassword
                || string.IsNullOrWhiteSpace(dto.Username))
            {
                AddError("Хатолик коди: 9066 - Паролни тасдиқланг // Код ошибки: 9066 - Подтвердите пароль", "Message");
                return;
            }

            string uniqueKey = _httpContext.Request.Cookies[$"login-key"]?.ToString();
            if (string.IsNullOrEmpty(uniqueKey))
            {
                AddError("Invalid key", "Message");
                return;
            }

            var user = _repository.ByUniqueKeyAndSmsCode(uniqueKey, dto.SmsCode);
            if (user == null || user.UserName != dto.Username)
            {
                AddError("Хатолик коди: 9062 - СМС код нотўғри // Код ошибки: 9062 - Код смс неверный", "Message");
                return;
            }

            var userIp = _httpContext.GetUserIP();
            var userAgent = string.Empty;

            if (_httpContext.Request.Headers.ContainsKey("User-Agent"))
                userAgent = _httpContext.Request.Headers["User-Agent"];

            _repository.RestoredUserPassword(dto);
            _repository.AddUserLog(UserLogAction.RestorePassword, user.PhoneNumber, user.Id, userIp, userAgent);

            if (HasErrors) return;

            if (IsValid)
                _unitOfWork.Save();

            try
            {
                _httpContext.Response.Cookies.Delete($"login-key", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _cookieConfig.Domain
                });
            }
            catch { }
        }
    }
}