using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IAccountRepository : IBaseEntityRepository<int, User>
    {
        User ByUserName(string userName);
        void ChangePassword(int userId, ChangePasswordDlDto dto);
        void ChangeLanguage(int userId, ChangeUserLanguageDlDto dto);
        UserLog AddUserLog(UserLogAction action, string userName, int? userId, string ipAddress, string userAgent);
        void UpdateUserLastAccessTime(int userId);
        void CreateUserDeviceLog(string userName, string uniqueKey, string smsCode, string userIp, string userAgent);
        User ByUniqueKeyAndSmsCode(string uniqueKey, string smsCode);
        void RestoredUserPassword(RestoredPasswordDlDto dto);
    }
}