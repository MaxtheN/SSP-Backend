using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Extensions;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.BizLogicLayer.UserServices;
using SspUis.Core;
using SspUis.Core.Configurations;
using SspUis.Core.Extensions;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.OneId;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet.Security;
using WEBASE.Integration.EImzo;
using WEBASE.Notify.Sms;
using WEBASE.Storage;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public class BusinessmanAccountService : StatusGenericHandler, IBusinessmanAccountService
    {
        private readonly IBusinessmanAccountRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly CookieConfig _cookieConfig;
        private readonly ISmsService _smsService;
        private readonly SmsProviderConfig _smsProviderConfig;
        private readonly IUserService _userService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEImzoService _eImzoService;
        private readonly ISoliqContractorService _soliqService;
        private readonly ISoliqContractorService _soliqContractorService;
        private readonly IPersonService _personService;
        private readonly IStorageService _storageService;
        private readonly IContractorService _contractorService;
        private readonly HttpContext _httpContext;
        private readonly INotBudgetContractorRepository _notBudgetContractorRepository;
        private readonly IntegrationAuthConfig _jwtAuthConfig;
        private readonly IIntegrationAuthService _integrationAuthService;
        private readonly IOneIdService _oneIdService;

        public BusinessmanAccountService(IUnitOfWork unitOfWork,
            IAuthService authService,
            IHttpContextAccessor httpContextAccessor,
            CookieConfig cookieConfig,
            ISmsService smsService,
            SmsProviderConfig smsProviderConfig,
            IBackgroundJobClient backgroundJobClient,
            IEImzoService eImzoService,
            ISoliqContractorService soliqService,
            ISoliqContractorService soliqContractorService,
            IPersonService personService,
            IStorageService storageService,
            IContractorService contractorService,
            INotBudgetContractorRepository notBudgetContractorRepository,
            IntegrationAuthConfig jwtAuthConfig,
            IIntegrationAuthService integrationAuthService,
            IUserService userService,
            IOneIdService oneIdService)
        {
            _repository = unitOfWork.MyAccountRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _cookieConfig = cookieConfig;
            _smsService = smsService;
            _smsProviderConfig = smsProviderConfig;
            _backgroundJobClient = backgroundJobClient;
            _eImzoService = eImzoService;
            _soliqService = soliqService;
            _soliqContractorService = soliqContractorService;
            _personService = personService;
            _storageService = storageService;
            _contractorService = contractorService;
            _httpContext = httpContextAccessor.HttpContext;
            _notBudgetContractorRepository = notBudgetContractorRepository;
            _jwtAuthConfig = jwtAuthConfig;
            _integrationAuthService = integrationAuthService;
            _userService = userService;
            _oneIdService = oneIdService;
        }

        #region CHANGE
        public async Task ChangePhoneNumber(BusinessmanUserSmsCodeDto dto)
        {
            var eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = eImzoTimeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var user = _unitOfWork.Context
                .Set<BusinessmanUserDeviceLog>()
                .FirstOrDefault(u => u.BusinessmanUserId == _authService.User.Id && u.SmsCode == dto.SmsCode);

            if (user == null)
            {
                AddError("Такого пользователя не существует !");
                return;
            }

            if (DateTime.Now > user.DateOfExpire)
            {
                AddError("Вы не подтвердили sms в течение указанного времени ! Попробуйте еще раз.");
                return;
            }

            _repository.ChangePhoneNumber(_authService.User.Id, new BusinessmanChangePhoneNumberDlDto { UserName = user.MobileNumber });
            CombineStatuses(_repository);

            if (IsValid)
                _unitOfWork.Save();
        }
        public void ChangePassword(BusinessmanChangePasswordDlDto dto)
        {
            _repository.ChangePassword(_authService.User.Id, dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        public void ChangeLanguage(ChangeBusinessmanUserLanguageDlDto dto)
        {
            _repository.ChangeLanguage(_authService.User.Id, dto);
            CombineStatuses(_repository);
            if (IsValid)
                _unitOfWork.Save();
        }
        public async Task RestorePassword(RestorePasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                AddError("Неверный логин");
                return;
            }

            dto.UserName = GetCorrectUserName(dto.UserName);
            var user = _repository.ByUserName(dto.UserName);
            if (user == null)
            {
                CombineStatuses(_repository);
                return;
            }

            var userIp = _httpContext.GetUserIP();
            var userAgent = string.Empty;

            if (_httpContext.Request.Headers.ContainsKey("User-Agent"))
                userAgent = _httpContext.Request.Headers["User-Agent"];

            string loginkey = Guid.NewGuid().ToString();
            _httpContext.Response.Cookies.Append("login-key", loginkey, new CookieOptions
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
            string verifycode = rnd.Next(1100, 9990).ToString();
            message = string.Format(message, user.UserName, verifycode);

            if (string.IsNullOrEmpty(message))
                message = verifycode;

            if (!dto.AppKeyHash.NullOrEmpty())
                message = $"{message} {dto.AppKeyHash}";

            await Task.Run(() => _smsService.Send(dto.UserName, verifycode));

            _repository.CreateUserDeviceLog(dto.UserName, loginkey, verifycode, userIp, userAgent);
            AddUserLog(UserLogAction.RestorePassword, dto.UserName, user.Id, userIp, userAgent);
        }
        public void RestorePasswordConfirm(RestorePasswordDlDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.NewPassword)
                || dto.NewPassword != dto.ConfirmedPassword
                || string.IsNullOrWhiteSpace(dto.UserName))
            {
                AddError("Хатолик коди: 9066 - Паролни тасдиқланг // Код ошибки: 9066 - Подтвердите пароль");
                return;
            }

            string loginkey = _httpContext.Request.Cookies["login-key"]?.ToString();
            if (string.IsNullOrEmpty(loginkey)) { AddError("Invalid key"); return; }

            dto.UserName = GetCorrectUserName(dto.UserName);
            var user = _repository.ByUniqueKeyAndSmsCode(loginkey, dto.SmsCode);

            if (user == null || user.UserName != dto.UserName)
            {
                AddError("Хатолик коди: 9062 - СМС код нотўғри // Код ошибки: 9062 - Код смс неверный");
                return;
            }

            var userIp = _httpContext.GetUserIP();
            var userAgent = string.Empty;

            if (_httpContext.Request.Headers.ContainsKey("User-Agent"))
                userAgent = _httpContext.Request.Headers["User-Agent"];

            _repository.RestoreUserPassword(dto);
            AddUserLog(UserLogAction.RestorePassword, dto.UserName, user.Id, userIp, userAgent);
            try
            {
                _httpContext.Response.Cookies.Delete("login-key", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _cookieConfig.Domain
                });
            }
            catch { }
        }
        #endregion

        #region AUTH
        //public BusinessmanAccountUserDto UpdateUserInfo(UpdateBusinessmanAccountUserDto dto)
        //{
        //    dto.Contractor.Id = _authService.Contractor.Id;
        //    using (var transaction = _unitOfWork.BeginTransaction())
        //    {

        //        var contractor = _unitOfWork.ContractorRepository.Update(dto.Contractor);
        //        CombineStatuses(_unitOfWork.ContractorRepository);
        //        if (HasErrors)
        //            return null;

        //        _repository.ChangeUserInfo(_authService.User.Id, dto);
        //        CombineStatuses(_repository);
        //        if (HasErrors)
        //            return null;
        //        _unitOfWork.Save();

        //        transaction.Commit();
        //    }
        //    return GetUserInfo();
        //}
        public BusinessmanAccountUserDto UpdateUserInfo(UpdateBusinessmanAccountUserDto dto)
        {
            try
            {
                dto.Contractor.Id = _authService.Contractor.Id;

                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    // Get the existing SettlementAccount IDs from the database
                    var existingSettlementAccountIds = _unitOfWork.Context.Set<Application>()
                        .Where(a => a.ContractorId == dto.Contractor.Id)
                        .Select(a => a.ContractorSettlementAccountId)
                        .ToList();

                    // Get the updated SettlementAccount IDs from the DTO
                    var updatedSettlementAccountIds = dto.Contractor.SettlementAccounts
                        .Select(account => account.Id)
                        .ToList();

                    // Find any IDs that exist in the existing accounts but not in the updated accounts
                    var deletedSettlementAccountIds = new List<long>();
                    foreach (var accountId in existingSettlementAccountIds)
                    {
                        if (accountId == null)
                        {
                            // Handle the null value (perhaps log it or skip it)
                            continue; // Move to the next iteration
                        }
                        if (!updatedSettlementAccountIds.Contains(accountId.Value))
                        {
                            deletedSettlementAccountIds.Add(accountId.Value);
                        }
                    }

                    if (deletedSettlementAccountIds.Any())
                    {

                        // If any deleted SettlementAccount IDs are found, add error and return
                        foreach (var accountId in deletedSettlementAccountIds)
                        {
                            var settlementAccount = _unitOfWork.Context.Set<ContractorSettlementAccount>().FirstOrDefault(a => a.Id == accountId);
                            AddError($"Bu {settlementAccount.AccountCode} hisob raqamni o'chira olmaysiz." , "Message");
                        }
                        return null;
                    }

                    // Update contractor information
                    var contractor = _unitOfWork.ContractorRepository.Update(dto.Contractor);
                    CombineStatuses(_unitOfWork.ContractorRepository);
                    if (HasErrors)
                        throw new Exception("Error updating contractor information.");

                    // Change user information
                    _repository.ChangeUserInfo(_authService.User.Id, dto);
                    CombineStatuses(_repository);
                    if (HasErrors)
                        throw new Exception("Error changing user information.");

                    _unitOfWork.Save();

                    transaction.Commit();
                }

                return GetUserInfo();
            }
            catch (Exception e)
            {
                AddError($"{e.Message}, {e.InnerException}, {e.HelpLink}, {e.Source}, {e.Data}, {e.HResult}, {e.StackTrace}");
                return null;
            }
        }
        public bool IsUserRegistered(IsBusinessmanUserRegisteredDto dto)
        {
            string requestId = GetUniqueID();
            _httpContext.Response.Cookies.Append("requestId", requestId, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddHours(1)
            });

            var businessman = _repository.ByUserName(GetCorrectUserName(dto.PhoneNumber));
            return businessman != null;
        }
        public async Task<BusinessmanLoginResultDto> Registrate(RegistrateBusinessmanDto dto)
        {
            string requestId = _httpContext.Request.Cookies["requestId"]?.ToString();

            if (string.IsNullOrEmpty(requestId))
            {
                AddError("Хатолик коди: 9067 - Сессия вақти тугади // Код ошибки: 9067 - Срок действия запроса истек.", "Message");
                return null;
            }
            //"D3D5B6C38F26484F9A5D5982860C1199"
            var smsCode = _repository.GetSmsCode(requestId);
            if (smsCode == null)
            {
                AddError("Хатолик коди: 9040 - Сессия вақти тугади. Телефон рақам топилмади // Код ошибки: 9040 - Срок действия запроса истек. Не найден номер телефона", "Message");
                return null;
            }

            var timeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : dto.Inn,
                Pinfl = dto.Pinfl,
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return null;

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = timeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : dto.Inn,
                Pinfl = dto.Pinfl
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return null;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var user = _repository.Registrate(dto, smsCode);
                    CombineStatuses(_repository);
                    if (HasErrors) return null;
                    _unitOfWork.Save();

                    await LoginInternal(user, true);
                    if (HasErrors) return null;

                    string trusteddevicekey = Guid.NewGuid().ToString();
                    _repository.CreateUserTrustedDeviceKeys(user.Id, trusteddevicekey, DateTime.Now.AddDays(_cookieConfig.Expires), _authService.UserIp, _authService.UserAgent);
                    AddUserLog(UserLogAction.LoginBySms, user.UserName, user.Id);
                    _httpContext.Response.Cookies.Append("trusted-device-sspmyuis-" + user.UserName, trusteddevicekey, new CookieOptions
                    {
                        HttpOnly = true,
                        Domain = _cookieConfig.Domain,
                        Expires = DateTimeOffset.Now.AddDays(_cookieConfig.Expires)
                    });
                    try
                    {
                        _httpContext.Response.Cookies.Delete("requestId", new CookieOptions
                        {
                            HttpOnly = true,
                            Domain = _cookieConfig.Domain,
                        });
                    }
                    catch
                    { }
                    transaction.Commit();
                    return new BusinessmanLoginResultDto
                    {
                        TrustedDevice = true,
                        PhoneNumber = user.UserName,
                        User = GetUserInfo(),
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<BusinessmanLoginResultDto> Login(BusinessmanLoginDto dto)
        {
            if (dto.PhoneNumber.NullOrWhiteSpace() || dto.Password.NullOrWhiteSpace())
            {
                AddError("Логин или пароль неверный", "Message");
                return null;
            }

            dto.PhoneNumber = GetCorrectUserName(dto.PhoneNumber);
            var user = _repository.ByUserName(dto.PhoneNumber);

            if (user == null || !user.IsValidPassword(dto.Password))
            {
                AddError("Логин или пароль неверный", "Message");

                if (user != null)
                    AddUserLog(UserLogAction.IncorrectPasswordEntered, user.UserName, user.Id);
                return null;
            }

            var signUpWithoutSms = await SignUpWithoutSms(user, dto.PhoneNumber, dto.Password);
            if (signUpWithoutSms != null) return signUpWithoutSms;

            var trusteddevicekey = _httpContext.Request.Cookies["trusted-device-sspmyuis-" + dto.PhoneNumber]?.ToString();
            if (!string.IsNullOrEmpty(trusteddevicekey) || dto.PhoneNumber.Equals(GetCorrectUserName("987654321")) || dto.PhoneNumber.Equals(GetCorrectUserName("123456789")))
            {
                var trustedDevices = _repository.GetUserTrustedDeviceKeys(user.Id);
                if (trustedDevices.Contains(trusteddevicekey))
                    _repository.UpdateUserLastAccessTime(user.Id);
                AddUserLog(UserLogAction.LoginByPassword, user.UserName, user.Id);

                if (user.BusinessmanUserInContractors.Where(x => x.StateId == StateIdConst.ACTIVE).Count() > 1)
                    return GetAuthInfo(user.Id, user.UserName);
                else
                {
                    await LoginInternal(user);
                    return new BusinessmanLoginResultDto
                    {
                        TrustedDevice = true,
                        PhoneNumber = dto.PhoneNumber,
                        User = GetUserInfo(),
                    };
                }
            }
            string loginkey = Guid.NewGuid().ToString();
            _httpContext.Response.Cookies.Append("login-key", loginkey, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddMinutes(20)
            });
            var rnd = new Random();
            string smsCode = rnd.Next(1100, 9990).ToString();

            string message = _smsProviderConfig.MessageTemplate.FirstOrDefault(x => x.Action == "onlinequeue")?.Text;
            if (string.IsNullOrEmpty(message))
                message = "{0}: {1}";

            message = string.Format(message, dto.PhoneNumber, smsCode);
            if (!dto.AppKeyHash.NullOrEmpty())
                message = $"{message} {dto.AppKeyHash}";

            _repository.CreateUserDeviceLog(dto.PhoneNumber, loginkey, smsCode, _authService.UserIp, _authService.UserAgent);
            CombineStatuses(_repository);
            if (HasErrors) return null;

            _unitOfWork.Save();
            await Task.Run(() => _smsService.Send(dto.PhoneNumber, message));

            return new BusinessmanLoginResultDto
            {
                TrustedDevice = false,
                PhoneNumber = dto.PhoneNumber
            };
        }
        public IntegrationLoginResultDto IntegrationLogin(IntegrationLoginDto dto)
        {
            if (dto.UserName.NullOrWhiteSpace() || dto.Password.NullOrWhiteSpace())
            {
                AddError("Имя пользователя или пароль неправильно" , "Message");
                return null;
            }

            var user = _jwtAuthConfig.IntegrationUsers
                .FirstOrDefault(a => a.UserName == dto.UserName);

            if (user == null || !user.IsValidPassword(dto.Password))
            {
                AddError("Имя пользователя или пароль неправильно", "Message");
                return null;
            }
            if (user.Inn.IsNullOrEmptyObject())
            {
                AddError("Inn mavjud emas", "Message");
                return null;
            }
            var contracts = _unitOfWork.Context.Set<MemshipCertificate>().Include(a => a.Contractor).Where(a => a.Contractor.Inn == user.Inn && a.StatusId == StatusIdConst.FORMED).ToList();
            if (contracts.Count() == 0 || contracts == null)
            {
                AddError("Sizda a'zolik shartnomasi mavjud emas", "Message");
                return null;
            }
            if (contracts.FirstOrDefault(a => a.ExpireOn.AddYears(1) >= DateTime.Now.AsDateOnly()) == null)
            {
                AddError("Sizning a'zolik shartnomangiz muddati o'tgan", "Message");
                return null;
            }

            if (IsValid)
            {
                string token = _integrationAuthService.GenerateToken(user.UserName);

                return new IntegrationLoginResultDto
                {
                    Token = token
                };
            }

            return null;
        }
        public async Task<BusinessmanLoginResultDto> OneIdLogin(OneIdLoginDto dto)
        {
            try
            {
                if (dto.Code.NullOrWhiteSpace())
                {
                    AddError("OneID code неправильно", "Message");
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
                var contractor = new Contractor();

                if (userData.LegalInfo.Count() != 0 && userData.LegalInfo.First().Tin != null)
                    contractor = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(con => con.Inn == userData.LegalInfo.FirstOrDefault().Tin && con.StateId != StateIdConst.PASSIVE);

                if ((contractor.Id.IsNullOrEmptyObject() || contractor.Id == 0) && !userData.Pin.IsNullOrEmptyObject())   
                    _unitOfWork.Context.Set<Contractor>().FirstOrDefault(con => con.Pinfl == userData.Pin && con.StateId != StateIdConst.PASSIVE);

                if (contractor == null)
                {
                    AddError($"Ushbu foydalanuvchida tadbirkorlik subyekti mavjud emas (Pin: {userData.Pin}, Inn: {userData.Tin})", "Message");
                    return null;
                }

                var busUserInContractor = _unitOfWork.Context.Set<BusinessmanUserInContractor>().FirstOrDefault(bus => bus.StateId != StateIdConst.PASSIVE && bus.ContractorId == contractor.Id);

                if (busUserInContractor == null)
                {
                    AddError($"Siz https://my.chamber.uz/ tizimida ro'yhatdan o'tmagansiz", "Message");
                    return null;
                }

                var user = _unitOfWork.Context.Set<BusinessmanUser>().FirstOrDefault(user => user.Id == busUserInContractor.BusinessmanUserId && user.StateId != StateIdConst.PASSIVE);

                if (user == null)
                {
                    AddError($"Siz https://my.chamber.uz/ tizimida ro'yhatdan o'tmagansiz", "Message");
                    return null;
                }

                if (IsValid && user != null)
                {
                    _repository.UpdateUserLastAccessTime(user.Id);

                    var trusteddevicekey = _httpContext.Request.Cookies["trusted-device-sspmyuis-" + user.UserName]?.ToString();

                    if (!string.IsNullOrEmpty(trusteddevicekey) || user.UserName.Equals(GetCorrectUserName(user.UserName)))
                    {
                        var trustedDevices = _repository.GetUserTrustedDeviceKeys(user.Id);

                        if (trustedDevices.Contains(trusteddevicekey))
                            _repository.UpdateUserLastAccessTime(user.Id);

                        AddUserLog(UserLogAction.LoginByEImzo, user.UserName, user.Id);

                        if (user.BusinessmanUserInContractors.Where(x => x.StateId == StateIdConst.ACTIVE && x.ContractorId == contractor.Id).Count() > 1)
                            return GetAuthInfo(user.Id, user.UserName);
                        else
                        {
                            await LoginInternal(user, false, contractor.Id);
                            return new BusinessmanLoginResultDto
                            {
                                TrustedDevice = true,
                                PhoneNumber = user.UserName,
                                User = GetUserInfo(),
                            };
                        }
                    }

                    string loginkey = Guid.NewGuid().ToString();

                    _httpContext.Response.Cookies.Append("login-key", loginkey, new CookieOptions
                    {
                        HttpOnly = true,
                        Domain = _cookieConfig.Domain,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.Now.AddMinutes(20)
                    });

                    _repository.CreateUserDeviceLog(user.UserName, loginkey, "Eimzo", _authService.UserIp, _authService.UserAgent);
                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    return new BusinessmanLoginResultDto
                    {
                        TrustedDevice = false,
                        PhoneNumber = user.UserName
                    };
                }
                await _oneIdService.Logout(accessToken.AccessToken);

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<EImzoChallangeResultDto> GetChallenge()
        {
            var challenge = await _eImzoService.Challenge();
            CombineStatuses(_eImzoService);
            return challenge;
        }
        public async Task<BusinessmanLoginResultDto> LoginByEImzo(LoginByEImzoBusinessmanDto dto)
        {
            var res = await _eImzoService.Auth(dto);
            CombineStatuses(_eImzoService);
            if (HasErrors)
                return null;

            var userIdentifier = res.SubjectCertificateInfo.SubjectName.Inn ?? res.SubjectCertificateInfo.SubjectName.Pinfl;

            if (!string.IsNullOrEmpty(res.SubjectCertificateInfo.SubjectName.Inn))
            {
                var contractorInn = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(con => con.Inn == userIdentifier && con.StateId != StateIdConst.PASSIVE);

                if (contractorInn == null)
                {
                    AddError($"Siz https://my.chamber.uz/ tizimida ro'yhatdan o'tmagansiz. Iltimos telefon qaramingiz orqali ro'yxatdan oting!", "Message");
                    return null;
                }

                var busUserInContractorInn = _unitOfWork.Context.Set<BusinessmanUserInContractor>().FirstOrDefault(bus => bus.StateId != StateIdConst.PASSIVE && bus.ContractorId == contractorInn.Id);

                if (busUserInContractorInn == null)
                {
                    AddError($"Siz https://my.chamber.uz/ tizimida ro'yhatdan o'tmagansiz. Iltimos telefon qaramingiz orqali ro'yxatdan oting!", "Message");
                    return null;
                }

                var userInn = _unitOfWork.Context.Set<BusinessmanUser>().FirstOrDefault(user => user.Id == busUserInContractorInn.BusinessmanUserId && user.StateId != StateIdConst.PASSIVE);

                if (userInn == null)
                {
                    AddError($"Siz https://my.chamber.uz/ tizimida ro'yhatdan o'tmagansiz. Iltimos telefon qaramingiz orqali ro'yxatdan oting!", "Message");
                    return null;
                }

                if (IsValid)
                {
                    _repository.UpdateUserLastAccessTime(userInn.Id);

                    var trusteddevicekey = _httpContext.Request.Cookies["trusted-device-sspmyuis-" + userInn.UserName]?.ToString();

                    if (!string.IsNullOrEmpty(trusteddevicekey) || userInn.UserName.Equals(GetCorrectUserName(userInn.UserName)))
                    {
                        var trustedDevices = _repository.GetUserTrustedDeviceKeys(userInn.Id);

                        if (trustedDevices.Contains(trusteddevicekey))
                            _repository.UpdateUserLastAccessTime(userInn.Id);

                        AddUserLog(UserLogAction.LoginByEImzo, userInn.UserName, userInn.Id);

                        if (userInn.BusinessmanUserInContractors.Where(x => x.StateId == StateIdConst.ACTIVE && x.ContractorId == contractorInn.Id).Count() > 1)
                            return GetAuthInfo(userInn.Id, userInn.UserName);
                        else
                        {
                            await LoginInternal(userInn, false, contractorInn.Id);
                            return new BusinessmanLoginResultDto
                            {
                                TrustedDevice = true,
                                PhoneNumber = userInn.UserName,
                                User = GetUserInfo(),
                            };
                        }
                    }

                    string loginkey = Guid.NewGuid().ToString();

                    _httpContext.Response.Cookies.Append("login-key", loginkey, new CookieOptions
                    {
                        HttpOnly = true,
                        Domain = _cookieConfig.Domain,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.Now.AddMinutes(20)
                    });

                    _repository.CreateUserDeviceLog(userInn.UserName, loginkey, "Eimzo", _authService.UserIp, _authService.UserAgent);
                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    return new BusinessmanLoginResultDto
                    {
                        TrustedDevice = false,
                        PhoneNumber = userInn.UserName
                    };
                }
            }
            else if (!string.IsNullOrEmpty(res.SubjectCertificateInfo.SubjectName.Pinfl))
            {
                var contractorPinfl = _unitOfWork.Context.Set<Contractor>().FirstOrDefault(con => con.Pinfl == userIdentifier && con.StateId != StateIdConst.PASSIVE);

                var busUserInContractor = _unitOfWork.Context.Set<BusinessmanUserInContractor>().FirstOrDefault(bus => bus.StateId != StateIdConst.PASSIVE && bus.ContractorId == contractorPinfl.Id);

                var userPinfl = _unitOfWork.Context.Set<BusinessmanUser>().FirstOrDefault(user => user.Id == busUserInContractor.BusinessmanUserId && user.StateId != StateIdConst.PASSIVE);

                if (userPinfl == null)
                {
                    AddError($"Foydalanuvchi topilmadi {userIdentifier}", "Message");
                    return null;
                }

                if (IsValid)
                {
                    _repository.UpdateUserLastAccessTime(userPinfl.Id);

                    var trusteddevicekey = _httpContext.Request.Cookies["trusted-device-sspmyuis-" + userPinfl.UserName]?.ToString();

                    if (!string.IsNullOrEmpty(trusteddevicekey) || userPinfl.UserName.Equals(GetCorrectUserName(userPinfl.UserName)))
                    {
                        var trustedDevices = _repository.GetUserTrustedDeviceKeys(userPinfl.Id);

                        if (trustedDevices.Contains(trusteddevicekey))
                            _repository.UpdateUserLastAccessTime(userPinfl.Id);

                        AddUserLog(UserLogAction.LoginByEImzo, userPinfl.UserName, userPinfl.Id);

                        if (userPinfl.BusinessmanUserInContractors.Where(x => x.StateId == StateIdConst.ACTIVE && x.ContractorId == contractorPinfl.Id).Count() > 1)
                            return GetAuthInfo(userPinfl.Id, userPinfl.UserName);
                        else
                        {
                            await LoginInternal(userPinfl, false, contractorPinfl.Id);
                            return new BusinessmanLoginResultDto
                            {
                                TrustedDevice = true,
                                PhoneNumber = userPinfl.UserName,
                                User = GetUserInfo(),
                            };
                        }
                    }

                    string loginkey = Guid.NewGuid().ToString();

                    _httpContext.Response.Cookies.Append("login-key", loginkey, new CookieOptions
                    {
                        HttpOnly = true,
                        Domain = _cookieConfig.Domain,
                        SameSite = SameSiteMode.Lax,
                        Expires = DateTimeOffset.Now.AddMinutes(20)
                    });
                    
                    _repository.CreateUserDeviceLog(userPinfl.UserName, loginkey, "Eimzo", _authService.UserIp, _authService.UserAgent);
                    CombineStatuses(_repository);
                    if (HasErrors) return null;

                    _unitOfWork.Save();

                    return new BusinessmanLoginResultDto
                    {
                        TrustedDevice = false,
                        PhoneNumber = userPinfl.UserName
                    };
                }
            }
            else
            {
                AddError("Foydalanuvchi topilmadi / yoki siz my.chamber.uz saytidan royhatdan otmagansiz", "Message");
                return null;
            }
            return null;
        }
        public async Task<BusinessmanLoginResultDto> SignInTwoFactor([FromBody] BusinessmanUserSmsCodeDto dto)
        {
            string loginkey = _httpContext.Request.Cookies["login-key"]?.ToString();

            if (string.IsNullOrEmpty(loginkey))
            {
                AddError("Invalid key");
                return null;
            }

            var user = _repository.ByUniqueKeyAndSmsCode(loginkey, dto.SmsCode);
            if (user == null)
            {
                AddError("Хатолик коди: 9062 - СМС код нотўғри // Код ошибки: 9062 - Код смс неверный", "Message");
                return null;
            }

            if (user.BusinessmanUserInContractors.Where(x => x.StateId == StateIdConst.ACTIVE).Count() > 1)
                return GetAuthInfo(user.Id, user.UserName);

            await LoginInternal(user);
            string trusteddevicekey = Guid.NewGuid().ToString();
            _repository.CreateUserTrustedDeviceKeys(user.Id, trusteddevicekey, DateTime.Now.AddDays(_cookieConfig.Expires), _authService.UserIp, _authService.UserAgent);
            AddUserLog(UserLogAction.LoginBySms, user.UserName, user.Id);
            _httpContext.Response.Cookies.Append("trusted-device-sspmyuis-" + user.UserName, trusteddevicekey, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                Expires = DateTimeOffset.Now.AddDays(_cookieConfig.Expires)
            });
            try
            {
                _httpContext.Response.Cookies.Delete("login-key", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _cookieConfig.Domain,
                });
            }
            catch { }
            return new BusinessmanLoginResultDto
            {
                TrustedDevice = true,
                PhoneNumber = user.UserName,
                User = GetUserInfo(),
            };
        }
        public BusinessmanAccountUserDto GetUserInfo()
        {
            var dto = _repository.ById<BusinessmanAccountUserDto>(_authService.User.Id);
            if(dto != null)
            {
				dto.ContractorId = _authService?.Contractor?.Id;
				if (dto.ContractorId.HasValue)
					dto.Contractor = _contractorService.Get(dto.ContractorId.Value);
			}
          
            return dto;
        }
        public List<ContractorSettlementAccountDto> GetContractorSettlementAccountList()
        {
            if (_authService.Contractor is null) return null;

            var data = _unitOfWork.Context.Set<ContractorSettlementAccount>()
                .Include(x => x.Bank).ThenInclude(x => x.Translates)
                .Include(x => x.State).ThenInclude(x => x.Translates)
                .Where(x => x.StateId != StateIdConst.PASSIVE && x.OwnerId == _authService.Contractor.Id)
                .ToList();

            return data.Select(x => new ContractorSettlementAccountDto
            {
                Id = x.Id,
                BankId = x.BankId,
                AccountCode = x.AccountCode,
                AccountName = x.AccountName,
                Bank = x.Bank.Translates.AsQueryable()
                    .FirstOrDefault(BankTranslate.GetExpr(BankTranslateColumn.bank_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? x.Bank.BankName,
                BankCode = x.Bank.Code,
                StateId = x.StateId,
                State = x.State.Translates.AsQueryable()
                    .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
                    ?.TranslateText ?? x.State.FullName,
                IsMain = x.IsMain
            }).ToList();
        }
        public async Task Logout()
        {
            try
            {
                if (!_authService.OneIdToken.NullOrEmpty())
                    await _oneIdService.Logout(_authService.OneIdToken);
                AddUserLog(UserLogAction.Logout, _authService.UserName, _authService.User.Id);
            }
            catch { }
            _authService.Logout();
        }
        public BusinessmanLoginResultDto SetContractorToAuth(SetContractorDto dto)
        {
            var businessman = _repository.ById(dto.BusinessmanUserId, true);

            var uniqueKey = _httpContext.Request.Cookies[$"unique-key"]?.ToString();
            if (string.IsNullOrEmpty(uniqueKey))
            {
                AddError("Invalid key");
                return null;
            }

            var log = _repository.GetByUnique(uniqueKey);
            if (log is null || log.BusinessmanUserId != dto.BusinessmanUserId)
            {
                AddError($"Не найдено {dto.BusinessmanUserId}!");
                return null;
            }

            if (!_repository.IsUserContractorPairActive(dto.BusinessmanUserId, dto.ContractorId))
            {
                AddError($"Бу {dto.BusinessmanUserId} бизнессманнинг бундай {dto.ContractorId} ли тадбиркорлиги мавжуд емас !!");
                return null;
            }

            _repository.UpdateContractorId(uniqueKey, dto.ContractorId);
            CombineStatuses(_repository);
            if (HasErrors) return null;

            if (IsValid) _unitOfWork.Save();

            _authService.Login(businessman.UserName);
            _authService.SelectContractor(dto.ContractorId);

            string trusteddevicekey = Guid.NewGuid().ToString();
            _repository.CreateUserTrustedDeviceKeys(businessman.Id, trusteddevicekey, DateTime.Now.AddDays(_cookieConfig.Expires), _authService.UserIp, _authService.UserAgent);
            AddUserLog(UserLogAction.LoginBySms, businessman.UserName, businessman.Id);
            _httpContext.Response.Cookies.Append("trusted-device-sspmyuis-" + businessman.UserName, trusteddevicekey, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                Expires = DateTimeOffset.Now.AddDays(_cookieConfig.Expires)
            });
            try
            {
                _httpContext.Response.Cookies.Delete($"unique-key", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _cookieConfig.Domain,
                });
            }
            catch { }

            return new BusinessmanLoginResultDto
            {
                TrustedDevice = true,
                PhoneNumber = businessman.UserName,
                User = GetUserInfo(),
            };
        }
        /// <summary>
        /// Andriod access uchun api my tarafga smssiz kira olishi kerak
        /// Login: 998901033685 Parol: 123456
        /// </summary>
        /// <returns></returns>
        private async Task<BusinessmanLoginResultDto> SignUpWithoutSms(BusinessmanUser user, string phoneNumber, string password)
        {
            string specialNumber = "998901033685";
            string specialPassword = "123456";

            if (phoneNumber == specialNumber && password == specialPassword)
            {
                await LoginInternal(user);
                return new BusinessmanLoginResultDto
                {
                    TrustedDevice = true,
                    PhoneNumber = phoneNumber,
                    User = GetUserInfo(),
                };
            }

            return null;
        }
        #endregion

        #region OTHER
        public async Task<string> GetHash()
        {
            var offer = await _unitOfWork.Context
                .Set<Offer>()
                .OrderByDescending(a => a.Id)
                .FirstOrDefaultAsync();

            if (offer is null)
                return null;

            return JsonSerializer.Serialize<string>($"{offer.Id}{offer.Name}{offer.Text}");
        }
        public async Task IsOffer(OfferDto dto)
        {
            var eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = eImzoTimeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : _authService.Contractor.Inn,
                Pinfl = _authService.Contractor.Pinfl
            });

            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                Guid signedDataFile = Guid.NewGuid();
                try
                {
                    var offer = _unitOfWork.Context.Set<Offer>().OrderByDescending(a => a.Id).FirstOrDefault();
                    _contractorService.CreateContractorOffers(new ContractorOfferDto
                    {
                        SignData = signedDataFile,
                        OfferId = offer.Id,
                        ContractorId = _authService.Contractor.Id
                    });

                    CombineStatuses(_contractorService);
                    if (HasErrors) return;

                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(eImzoTimeStamp.Pkcs7b64));
                    _storageService.Save(DocumentStorageConst.SIGN_DATA, _authService.User != null
                        ? _authService.User.Id.ToString()
                        : _authService.Contractor.Id.ToString(), new StorageFile(signedDataFile, "signData.txt", ms));

                    CombineStatuses(_storageService);
                    if (HasErrors)
                    {
                        transaction.Rollback();
                        return;
                    }

                    if (IsValid)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                    if (ex.InnerException != null) AddError(ex.InnerException.Message);
                    transaction.Rollback();
                    return;
                }
            }
        }
        public async Task SendSMSCode(IsBusinessmanUserRegisteredDto dto)
        {
            if (_unitOfWork.Context.Set<BusinessmanUser>().Any(u => u.UserName == dto.PhoneNumber))
            {
                AddError("Этот номер телефона используется.");
                return;
            }
            if (dto.PhoneNumber.NullOrWhiteSpace())
            {
                AddError("Хатолик коди: 9060 - Телефон рақамни киритинг // Код ошибки: 9060 - Введите номер мобильного телефона!");
                return;
            }

            string trusteddevicekey = Guid.NewGuid().ToString();
            _repository.CreateUserTrustedDeviceKeys(_authService.User.Id, trusteddevicekey, DateTime.Now.AddDays(_cookieConfig.Expires), _authService.UserIp, _authService.UserAgent);

            string smscode = new Random().Next(1200, 9999).ToString();
            var deviceLog = new BusinessmanUserDeviceLog
            {
                IpAdress = _authService.UserIp,
                UserAgent = _authService.UserAgent,
                UniqueKey = trusteddevicekey,
                SmsCode = smscode,
                DateOfExpire = DateTime.Now.AddMinutes(5),
                MobileNumber = dto.PhoneNumber,
                BusinessmanUserId = _authService.User.Id
            };
            _unitOfWork.Context.Set<BusinessmanUserDeviceLog>().Add(deviceLog);
            _unitOfWork.Context.Entry(deviceLog).State = EntityState.Added;
            _unitOfWork.Save();

            dto.PhoneNumber = GetCorrectUserName(dto.PhoneNumber);
            string message = _smsProviderConfig.MessageTemplate.FirstOrDefault(x => x.Action == "onlinequeue")?.Text;
            if (string.IsNullOrEmpty(message))
                message = "{0}: {1}";

            message = string.Format(message, dto.PhoneNumber, smscode);

            await Task.Run(() => _smsService.Send(dto.PhoneNumber, message));
        }
        public async Task SendSMSCode(BusinessmanUserVerifyCodeDto dto)
        {
            Random rnd = new Random();
            string requestId = _httpContext.Request.Cookies["requestId"]?.ToString();

            if (string.IsNullOrEmpty(requestId))
            {
                AddError("Хатолик коди: 9067 - Сессия вақти тугади // Код ошибки: 9067 - Срок действия запроса истек.");
                return;
            }

            if (string.IsNullOrEmpty(dto.PhoneNumber))
            {
                AddError("Хатолик коди: 9060 - Телефон рақамни киритинг // Код ошибки: 9060 - Введите номер мобильного телефона!");
                return;
            }
            if (dto.Password != dto.PasswordConfirm || string.IsNullOrWhiteSpace(dto.Password))
            {
                AddError("Хатолик коди: 9061 - Пароль кўрсатилмаган // Код ошибки: 9061 - Пароль не указан.");
                return;
            }

            dto.PhoneNumber = GetCorrectUserName(dto.PhoneNumber);
            string smscode = rnd.Next(1200, 9999).ToString();
            var passwordSalt = HashHelper.CreateRandomSalt();
            var passwordHash = new CustomPaswordHasher().HashPassword(dto.Password, passwordSalt);

            _repository.AddSmsCode(dto.PhoneNumber, passwordSalt, passwordHash, smscode, requestId);
            _unitOfWork.Save();
            string message = _smsProviderConfig.MessageTemplate.FirstOrDefault(x => x.Action == "onlinequeue")?.Text;
            if (string.IsNullOrEmpty(message))
                message = "{0}: {1}";

            message = string.Format(message, dto.PhoneNumber, smscode);

            await Task.Run(() => _smsService.Send(dto.PhoneNumber, message));
        }
        public bool IsValidSMSCode(BusinessmanUserSmsCodeDto dto)
        {
            string requestId = _httpContext.Request.Cookies["requestId"]?.ToString();

            if (string.IsNullOrEmpty(requestId))
            {
                AddError("Срок действия запроса истек.");
                return false;
            }
            if (!_unitOfWork.Context.Set<SmsCode>().Any(a => a.RequestId == requestId
                                                          && a.Code == dto.SmsCode
                                                          && a.ExpireDate > DateTime.Now))
            {
                AddError("Хатолик коди: 9062 - СМС код нотўғри // Код ошибки: 9062 - Код смс неверный");
            }
            return IsValid;
        }
        public async Task<BusinessmanAccountUserDto> SelectContractor(string inn, string pinfl = null)
        {
            var result = await GetContractorsList();
            CombineStatuses(_soliqContractorService);
            if (HasErrors)
                return null;
            if (!result.Any(a => a.Tin == inn))
            {
                AddError("Неверный инн");
                return null;
            }

            var user = _repository.ById(_authService.User.Id);
            var canDispose = _unitOfWork.CurrentTransaction == null;
            var transaction = _unitOfWork.CurrentTransaction ?? _unitOfWork.BeginTransaction();
            try
            {
                var contractor = _unitOfWork.ContractorRepository.ByInn(inn);
                if (contractor == null)
                {
                    var dto = await _contractorService.GetByInn(inn);

                    if (dto == null)
                        dto = await _contractorService.GetByPinfl(pinfl);

                    if (dto == null)
                    {
                        AddError($"Солиқдан малумот инн: {inn} ва пинфл: {pinfl} ҳар иккаласида келмади");
                        return null;
                    }

                    contractor = _unitOfWork.ContractorRepository.Create(dto);
                    CombineStatuses(_unitOfWork.ContractorRepository);
                    if (HasErrors)
                        return null;
                    _unitOfWork.Save();
                }

                if (!user.BusinessmanUserInContractors.Any(a => a.ContractorId == contractor.Id))
                {
                    user.BusinessmanUserInContractors.Add(new BusinessmanUserInContractor
                    {
                        ContractorId = contractor.Id,
                        StateId = StateIdConst.ACTIVE
                    });
                    _unitOfWork.Context.Entry(user).State = EntityState.Modified;
                    _unitOfWork.Save();
                }

                if (canDispose)
                    transaction.Commit();
                _authService.SelectContractor(contractor.Id);
                return GetUserInfo();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (canDispose)
                    transaction.Dispose();
            }
        }
        public async Task<List<ByDirectorTinFactura>> GetContractorsList()
        {
            var user = _repository.ById(_authService.User.Id);
            List<ByDirectorTinFactura> result;

            if (user.IsPinfl)
            {
                var res = await _soliqContractorService.GetByPinfl(_authService.User.Pinfl);
                if (res == null) return null;

                result = new List<ByDirectorTinFactura> { new ByDirectorTinFactura
                {
                    Name = res.Name,
                    Tin = res.Tin?.ToString() ?? string.Empty,
                    Pinfl = _authService.User.Pinfl
                }};
            }
            else
            {
                var contractor = await _soliqContractorService.GetByInn(_authService.User.Inn);
                CombineStatuses(_soliqContractorService);
                if (HasErrors) return null;

                result = new List<ByDirectorTinFactura> { new ByDirectorTinFactura
                {
                    Name = contractor.Company.Name,
                    Tin = contractor.Company.Tin,
                }};
            }

            CombineStatuses(_soliqContractorService);
            return result;
        }
        public ContractorInfoDto GetContractorInfo()
        {
            var checkContractor = _notBudgetContractorRepository.ByInn(_authService.Contractor.Inn);

            var startDate = _authService.Contractor.RegistrationDate.ToDateTime(TimeOnly.MaxValue);
            var endDate = DateTime.Now;

            // Calculate the difference in years, months, and days
            int yearsDiff = endDate.Year - startDate.Year;
            int monthsDiff = endDate.Month - startDate.Month;
            int daysDiff = endDate.Day - startDate.Day;

            // Adjust for negative differences
            if (monthsDiff < 0 || (monthsDiff == 0 && daysDiff < 0))
            {
                yearsDiff--;
                monthsDiff += 12;
            }

            if (daysDiff < 0)
            {
                monthsDiff--;
                daysDiff += DateTime.DaysInMonth(startDate.Year, startDate.Month);
            }


            return new ContractorInfoDto
            {
                IsBudget = checkContractor != null,
                RegistrationDate = _authService.Contractor.RegistrationDate,
                DifferenceInYears = yearsDiff,
                DifferenceInMonths = monthsDiff,
                DifferenceInDays = daysDiff,
                MoreThan2Years = yearsDiff >= 2,
                CanCreate = checkContractor == null && yearsDiff >= 2
            };
        }
        public ContractorDocumentInfo GetContractorDocumentInfo()
        {
            var result = new ContractorDocumentInfo();
            //var dateForNew = new DateOnly(2023, 10, 1);
            result.HasPrtnApplication = _unitOfWork.ApplicationRepository.AllAsQueryable
                .Any(a => a.ApplicationTypeId == ApplicationTypeIdConst.PARTNER);

            result.HasPrtnContract = _unitOfWork.PrtnContractRepository.AllAsQueryable.Any();

            result.HasPrtnCertificate = _unitOfWork.PrtnCertificateRepository.AllAsQueryable.Any();
            result.HasMemshipApplication = _unitOfWork.ApplicationRepository.AllAsQueryable
                .Any(a => a.ApplicationTypeId == ApplicationTypeIdConst.MEMSHIP);
            result.HasMemshipContract = _unitOfWork.MemshipContractRepository.AllAsQueryable/*.Where(x => x.DocOn >= dateForNew)*/.Any();
            result.HasMemshipCertificate = _unitOfWork.MemshipCertificateRepository.AllAsQueryable.Any();

            result.HasClaimApplication = _unitOfWork.ApplicationRepository.AllAsQueryable
            .Any(a => a.ApplicationTypeId == ApplicationTypeIdConst.CLAIM);
            result.HasClaimMediationPlan = _unitOfWork.MediationPlanRepository.AllAsQueryable.Any();
            result.HasClaimMediation = _unitOfWork.MediationRepository.AllAsQueryable.Any();
            result.HasPrtnCreditDemand = _unitOfWork.PrtnCreditDemandRepository.AllAsQueryable.Any();
            result.HasAdditionalAgreement = _unitOfWork.AdditionalAgreementRepository.AllAsQueryable
                .Where(x => x.StatusId == StatusIdConst.SIGNING
                || x.StatusId == StatusIdConst.SIGNED).Any();
            result.PrntContractTypeId = _unitOfWork.Context.Set<PrtnApplication>()
                .FirstOrDefault(a => a.Application.ContractorId == _authService.Contractor.Id)
                ?.PrtnContractTypeId != null ? _unitOfWork.Context.Set<PrtnApplication>()
                .FirstOrDefault(a => a.Application.ContractorId == _authService.Contractor.Id).PrtnContractTypeId : null;
            return result;
        }
        public ContractorMemshipStateDto GetContractorMemshipState()
        {
            var result = new ContractorMemshipStateDto();
            var now = DateOnly.FromDateTime(DateTime.Now);
            if (_unitOfWork.MemshipCertificateRepository.AllAsQueryable.Any(x => x.ExpireOn >= now))
                result.IsMember = true;
            else
                result.IsMember = false;
            var contract = _unitOfWork.MemshipContractRepository.AllAsQueryable
                .Include(x => x.MemshipContractType)
                .OrderByDescending(x => x.DocOn)
                .FirstOrDefault();
            if (contract != null)
            {
                result.State = contract.MemshipContractType.FullName;
            }
            else
                result.State = "-";

            var bhm = _unitOfWork.FixedMinimumValueRepository.AllAsQueryable.FirstOrDefault(x => x.MinimumValueTypeId == MinimumValueTypeIdConst.BRV);
            result.Balance = _unitOfWork.MemshipPaymentOrderRepository.AllAsQueryable.Sum(x => x.Amount);
            result.Balance -= (_unitOfWork.MemshipContractRepository.AllAsQueryable.FirstOrDefault()?.BaseFixedMinimumValue ?? 0) * (bhm != null ? bhm.FixedValue : 0);
            return result;
        }
        public async Task SyncWithTax()
        {
            await _contractorService.UpdateFromSoliq(_authService.Contractor.Id);
        }
        public async Task<SoliqContractorByTinDto> GetFromTax(string inn)
        {
            var dto = await _soliqService.GetByInn(inn);
            return dto;
        }
        public async Task<SoliqContractorDebtByPinflDto> GetFromTaxByPinfl(string pinfl)
        {
            var dto = await _soliqService.GetByPinfl(pinfl);
            return dto;
        }
        public List<ContractorInfoListDto> GetContractorListByUserId(int businessmanUserId)
        {
            var contractor = _unitOfWork.Context.Set<BusinessmanUserInContractor>()
                .Where(x => x.StateId != StateIdConst.PASSIVE
                    && x.BusinessmanUserId == businessmanUserId);

            if (contractor is null)
            {
                AddError("Бу тадбиркорнинг ҳеч қандай корхонаси мавжуд емас !");
                return null;
            }

            return contractor.Select(x => new ContractorInfoListDto
            {
                Id = x.ContractorId,
                FullName = x.Contractor.FullName,
                Inn = x.Contractor.Inn,
                Pinfl = x.Contractor.Pinfl,
                CountryId = x.Contractor.CountryId,
                Country = x.Contractor.Country.Translates.AsQueryable()
                    .FirstOrDefault(CountryTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? x.Contractor.Country.FullName,
                RegionId = x.Contractor.RegionId,
                Region = x.Contractor.Region.Translates.AsQueryable()
                    .FirstOrDefault(RegionTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? x.Contractor.Region.FullName,
                DistrictId = x.Contractor.DistrictId,
                District = x.Contractor.District.Translates.AsQueryable()
                    .FirstOrDefault(DistrictTranslate.GetExpr(
                        TranslateColumn.full_name,
                        ServiceProvider.CultureHelper.CurrentCulture.Id))
                    .TranslateText ?? x.Contractor.District.FullName,
                Director = x.Contractor.Director,
                RegistrationDate = x.Contractor.RegistrationDate,
                RegistrationNumber = x.Contractor.RegistrationNumber
            }).ToList();
        }
        public void DeactivateAssociation(DeactivateAssociationDto dto)
        {
            if (dto.ContractorId == _authService.Contractor.Id)
            {
                AddError("Сиз турган ташкилотингизни ўчира олмайсиз !");
                return;
            }

            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.SmsCode))
            {
                AddError("Неверный логин !");
                return;
            }

            string loginKey = _httpContext.Request.Cookies["login-key"]?.ToString();
            if (string.IsNullOrEmpty(loginKey))
            {
                AddError("Invalid key");
                return;
            }

            var businessman = _repository.ByUniqueKeyAndSmsCode(loginKey, dto.SmsCode);
            dto.Username = GetCorrectUserName(dto.Username);
            if (businessman == null || businessman.UserName != dto.Username || businessman.Id != _authService.User.Id)
            {
                AddError("Хатолик коди: 9062 - СМС код нотўғри // Код ошибки: 9062 - Код смс неверный");
                return;
            }

            var entity = _unitOfWork.Context.Set<BusinessmanUserInContractor>()
                .IsActive()
                .FirstOrDefault(x => x.ContractorId == dto.ContractorId && x.BusinessmanUserId == businessman.Id);

            if (entity is not null)
            {
                entity.Passive(ref entity);

                _unitOfWork.Context.Entry(entity).State = EntityState.Modified;
                _unitOfWork.Save();
            }
            else
            {
                AddError($"Хатолик коди: 9063 - Сизда бундай тадбиркорлик субекти мавжуд емас !");
                return;
            }

            try
            {
                _httpContext.Response.Cookies.Delete($"login-key", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = _cookieConfig.Domain,
                });
            }
            catch { }
        }
        public async Task AddNewContractor(ToAddOrganizationDto dto)
        {
            var eImzoTimeStamp = await _eImzoService.TimeStamp(new EImzoTimeStampDto
            {
                SignData = dto.SignedData,
                Inn = dto.IsPinfl ? null : dto.Inn,
                Pinfl = dto.Pinfl
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var eImzoVerifyAttached = await _eImzoService.VerifyAttached(new EImzoVerifyDto
            {
                SignData = eImzoTimeStamp.Pkcs7b64,
                Inn = dto.IsPinfl ? null : dto.Inn,
                Pinfl = dto.Pinfl
            });
            CombineStatuses(_eImzoService);
            if (HasErrors) return;

            var user = _repository.ById(_authService.User.Id);
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var contractor = dto.IsPinfl
                        ? _unitOfWork.ContractorRepository.ByPinfl(dto.Pinfl)
                        : _unitOfWork.ContractorRepository.ByInn(dto.Inn);

                    if (contractor is null)
                    {
                        var contractorFromTax = dto.IsPinfl
                            ? await _contractorService.GetByPinfl(dto.Pinfl)
                            : await _contractorService.GetByInn(dto.Inn);

                        if (contractorFromTax is null
                            || contractorFromTax.CountryId == default
                            || contractorFromTax.RegionId == default
                            || contractorFromTax.DistrictId == default)
                        {
                            AddError($"Солиқдан малумот келишда хато !");
                            return;
                        }

                        contractor = _unitOfWork.ContractorRepository.Create(contractorFromTax);
                        CombineStatuses(_unitOfWork.ContractorRepository);
                        if (HasErrors) return;
                        _unitOfWork.Save();
                    }

                    if (!_unitOfWork.Context.Set<BusinessmanUserInContractor>()
                        .Any(a => a.ContractorId == contractor.Id && a.StateId != StateIdConst.PASSIVE))
                    {
                        user.BusinessmanUserInContractors.Add(new BusinessmanUserInContractor
                        {
                            BusinessmanUserId = _authService.User.Id,
                            ContractorId = contractor.Id,
                            StateId = StateIdConst.ACTIVE,
                            CreatedAt = DateTime.Now
                        });
                        _unitOfWork.Context.Entry(user).State = EntityState.Modified;
                        _unitOfWork.Save();
                    }
                    else
                    {
                        AddError($"Бу Contractor:{contractor.FullName} бошқа номер орқали рўйҳатдан ўтган");
                        return;
                    }

                    if (HasErrors) return;

                    if (IsValid)
                        transaction.Commit();
                }
                catch (Exception ex)
                {
                    AddError(ex.Message);
                    if (ex.InnerException is not null) AddError(ex.InnerException.Message);

                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }
        #endregion

        #region HELPER
        private void AddUserLog(UserLogAction action, string userName, int? userId, string userIp = null, string userAgent = null)
        {
            _repository.AddUserLog(action, userName, userId, userIp ?? _authService.UserIp, userAgent ?? _authService.UserAgent);
            _unitOfWork.Save();
        }
        private static string GetUniqueID()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }
        private async Task LoginInternal(BusinessmanUser user, bool isRegistrate = false, long? contractorId = null)
        {
            _authService.Login(user.UserName);
            if (isRegistrate)
            {
                try
                {
                    var contractors = await GetContractorsList();
                    if (HasErrors)
                        return;
                    if (contractors != null && contractors.Count == 1)
                    {
                        await SelectContractor(contractors[0].Tin, contractors[0].Pinfl);
                    }
                }
                catch (Exception e)
                {
                    AddError($"Ошибка подключения к налоговой системе | ({e.Message}) | ({e.InnerException})");
                }
            }
            else
            {
                var conId = _repository.Context.Set<BusinessmanUserInContractor>()
                                                      .IsActive().Where(a => a.BusinessmanUserId == _authService.User.Id)
                                                      .Select(a => a.ContractorId)
                                                      .FirstOrDefault(a => (contractorId == null || a == contractorId));
                _authService.SelectContractor(conId);
            }
        }
        private static string GetCorrectUserName(string phoneNumber)
        {
            return BusinessmanUser.GetCorrectUserName(phoneNumber);
        }
        private BusinessmanLoginResultDto GetAuthInfo(int businessmanUserId, string phoneNumber)
        {
            var uniqueKey = Core.Extensions.Helper.RndUniqueKey(Core.Extensions.Helper.DEFAULT_LENGTH);

            _httpContext.Response.Cookies.Append($"unique-key", uniqueKey, new CookieOptions
            {
                HttpOnly = true,
                Domain = _cookieConfig.Domain,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Now.AddMinutes(20)
            });

            _repository.CreateBusinessmanUserContractorLog(new BusinessmanUserContractorLogDlDto
            {
                DateOfExpire = DateTime.Now.AddMinutes(20),
                BusinessmanUserId = businessmanUserId,
                UniqueKey = uniqueKey,
                ContractorId = null
            });

            CombineStatuses(_repository);
            if (HasErrors) return null;

            _unitOfWork.Save();

            return new BusinessmanLoginResultDto
            {
                TrustedDevice = true,
                PhoneNumber = phoneNumber,
                BusinessmanUserId = businessmanUserId,
                User = null
            };
        }
        #endregion
    }
}