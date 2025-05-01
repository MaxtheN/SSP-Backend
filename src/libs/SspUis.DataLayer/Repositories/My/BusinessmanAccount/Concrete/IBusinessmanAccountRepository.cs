using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IBusinessmanAccountRepository : IBaseEntityRepository<int, BusinessmanUser>
    {
        BusinessmanUser ByUserName(string userName);
        SmsCode AddSmsCode(string phoneNumber, string passwordSalt, string passwordHash, string smscode, string requestId);
        void ChangePassword(int id, BusinessmanChangePasswordDlDto dto);
        void ChangePhoneNumber(int id, BusinessmanChangePhoneNumberDlDto dto);
        BusinessmanUserLog AddUserLog(UserLogAction action, string userName, int? userId, string ipAddress, string userAgent);
        void ChangeLanguage(int id, ChangeBusinessmanUserLanguageDlDto dto);
        void UpdateUserLastAccessTime(int userId);
        void CreateUserDeviceLog(string userName, string loginkey, string verifycode, string userIp, string userAgent);
        BusinessmanUser ByUniqueKeyAndSmsCode(string loginkey, string smsCode);
        void RestoreUserPassword(RestorePasswordDlDto dto);
        List<string> GetUserTrustedDeviceKeys(int businessmanUserId);
        void CreateUserTrustedDeviceKeys(int businessmanUserId, string uniqueKey, DateTime dateOfExpire, string userIp, string userAgent);
        BusinessmanUser Registrate(RegistrateBusinessmanUserDlDto dto, SmsCode smsCode);
        SmsCode GetSmsCode(string requestId);
        void ChangeUserInfo(int id, UpdateUserInfoDlDto dto);
        void CreateBusinessmanUserContractorLog(BusinessmanUserContractorLogDlDto dto);
        bool IsUserContractorPairActive(int businessmanUserId, long contractorId);
        BusinessmanUserContractorLog GetByUnique(string uniqueKey);
        void UpdateContractorId(string uniqueKey, long contractorId);
    }
}
