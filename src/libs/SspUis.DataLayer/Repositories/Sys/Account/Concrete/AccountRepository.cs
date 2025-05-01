using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AccountRepository : BaseEntityRepository<int, User>, IAccountRepository
    {
        public AccountRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }

        public void ChangePassword(int id, ChangePasswordDlDto dto)
        {
            var entity = ById(id);

            if (dto.NewPassword.NullOrEmpty())
                AddError("NewPassword is empty");
            else if (!entity.IsValidPassword(dto.CurrentPassword))
                AddError("Текущий пароль неправильно введено");

            if (IsValid)
            {
                dto.UpdateEntity(entity);
            }
        }

        public void ChangeLanguage(int id, ChangeUserLanguageDlDto dto)
        {
            var entity = ById(id);
            dto.UpdateEntity(entity);
        }

        public UserLog AddUserLog(UserLogAction action, string userName, int? userId, string ipAddress, string userAgent)
        {
            var userLog = new UserLog
            {
                ActionName = action.ToString(),
                CreatedAt = DateTime.Now,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                UserName = userName,
                UserId = userId
            };
            Context.Set<UserLog>().Add(userLog);
            Context.Entry(userLog).State = EntityState.Added;
            return userLog;
        }

        public void UpdateUserLastAccessTime(int userId)
        {
            var user = ById(userId);
            user.LastAccessTime = DateTime.Now;
            Context.Entry(user).State = EntityState.Modified;
        }

        public User ByUserName(string userName)
        {
            User user = Context.Set<User>()
                .IsActive()
                .FirstOrDefault(x => x.UserName == userName);

            if (user is null)
                AddEntityNotFoundError();

            return user;
        }

        public void CreateUserDeviceLog(string userName, string uniqueKey, string smsCode, string userIp, string userAgent)
        {
            User user = this.ByUserName(userName);
            if (user == null)
            {
                AddEntityNotFoundError();
                return;
            }

            var deviceLog = new UserDeviceLog
            {
                UserId = user.Id,
                UniqueKey = uniqueKey,
                IpAddress = userIp,
                UserAgent = userAgent,
                DateOfExpire = DateTime.Now.AddMinutes(20),
                PhoneNumber = userName,
                SmsCode = smsCode,
            };

            Context.Set<UserDeviceLog>().Add(deviceLog);
            Context.Entry(deviceLog).State = EntityState.Added;
        }

        public User ByUniqueKeyAndSmsCode(string uniqueKey, string smsCode)
        {
            var deviceLog = Context.Set<UserDeviceLog>()
                                   .FirstOrDefault(a => a.UniqueKey == uniqueKey
                                                     && a.SmsCode == smsCode
                                                     && a.DateOfExpire > DateTime.Now);
            if (deviceLog == null)
            {
                AddEntityNotFoundError();
                return null;
            }

            return ById(deviceLog.UserId);
        }

        public void RestoredUserPassword(RestoredPasswordDlDto dto)
        {
            var entity = this.ByUserName(dto.Username);

            if (dto.NewPassword.NullOrEmpty())
                AddError("NewPassword is empty");

            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
    }
}