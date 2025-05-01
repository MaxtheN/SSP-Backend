using GenericServices;
using Microsoft.EntityFrameworkCore;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class BusinessmanAccountRepository : BaseEntityRepository<int, BusinessmanUser>, IBusinessmanAccountRepository
    {
        public BusinessmanAccountRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public SmsCode AddSmsCode(string phoneNumber, string passwordSalt, string passwordHash, string smscode, string requestId)
        {
            var smsCode = new SmsCode
            {
                PhoneNumber = phoneNumber,
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash,
                Code = smscode,
                RequestId = requestId,
                ExpireDate = DateTime.Now.AddHours(1)
            };
            Context.Set<SmsCode>().Add(smsCode);
            Context.Entry(smsCode).State = EntityState.Added;
            return smsCode;
        }

        public SmsCode GetSmsCode(string requestId)
        {
            return Context.Set<SmsCode>().OrderByDescending(a => a.CreatedAt)
                                         .FirstOrDefault(a => a.RequestId == requestId
                                                           && a.ExpireDate > DateTime.Now);
        }

        public BusinessmanUser ByUserName(string userName)
        {
            var entity = ByIdQuery().FirstOrDefault(a => a.UserName == userName);
            if (entity == null)
                AddEntityNotFoundError();
            return entity;
        }

        protected override IQueryable<BusinessmanUser> ByIdQuery()
        {
            return base.ByIdQuery().Include(a => a.BusinessmanUserInContractors);
        }

        public void ChangePassword(int id, BusinessmanChangePasswordDlDto dto)
        {
            var entity = ById(id);

            if (dto.NewPassword.NullOrEmpty())
                AddError("NewPassword is empty");
            else if (!entity.IsValidPassword(dto.CurrentPassword))
                AddError("Текущий пароль неправильно введено");

            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void ChangePhoneNumber(int id, BusinessmanChangePhoneNumberDlDto dto)
        {
            var entity = ById(id);

            if (dto.UserName.NullOrEmpty())
                AddError("Хатолик коди: 9060 - Телефон рақамни киритинг // Код ошибки: 9060 - Введите номер мобильного телефона!");

            if(IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void ChangeLanguage(int id, ChangeBusinessmanUserLanguageDlDto dto)
        {
            var entity = ById(id);
            dto.UpdateEntity(entity);
        }

        public BusinessmanUserLog AddUserLog(UserLogAction action, string userName, int? userId, string ipAddress, string userAgent)
        {
            var userLog = new BusinessmanUserLog
            {
                ActionName = action.ToString(),
                CreatedAt = DateTime.Now,
                UserAgent = userAgent,
                IpAddress = ipAddress,
                UserName = userName,
                UserId = userId
            };
            Context.Set<BusinessmanUserLog>().Add(userLog);
            Context.Entry(userLog).State = EntityState.Added;
            return userLog;
        }

        public void UpdateUserLastAccessTime(int userId)
        {
            var user = ById(userId);
            user.LastAccessTime = DateTime.Now;
            Context.Entry(user).State = EntityState.Modified;
        }

        public void CreateUserDeviceLog(string userName, string uniqueKey, string smsCode, string userIp, string userAgent)
        {
            var user = ByUserName(userName);
            if (user == null)
            {
                AddEntityNotFoundError();
                return;
            }
            var deviceLog = new BusinessmanUserDeviceLog
            {
                IpAdress = userIp,
                UserAgent = userAgent,
                UniqueKey = uniqueKey,
                SmsCode = smsCode,
                DateOfExpire = DateTime.Now.AddMinutes(20),
                MobileNumber = userName,
                BusinessmanUserId = user.Id
            };
            Context.Set<BusinessmanUserDeviceLog>().Add(deviceLog);
            Context.Entry(deviceLog).State = EntityState.Added;
        }

        public BusinessmanUser ByUniqueKeyAndSmsCode(string uniquekey, string smsCode)
        {
            var deviceLog = Context.Set<BusinessmanUserDeviceLog>()
                                   .FirstOrDefault(a => a.UniqueKey == uniquekey
                                                     && a.SmsCode == smsCode
                                                     && a.DateOfExpire > DateTime.Now);
            if (deviceLog == null)
            {
                AddEntityNotFoundError();
                return null;
            }
            return ById(deviceLog.BusinessmanUserId);
        }

        public void RestoreUserPassword(RestorePasswordDlDto dto)
        {
            var entity = ByUserName(dto.UserName);
            if (dto.NewPassword.NullOrEmpty())
                AddError("NewPassword is empty");
            if (IsValid)
            {
                dto.UpdateEntity(entity);
                Context.Entry(entity).State = EntityState.Modified;
            }

        }

        public List<string> GetUserTrustedDeviceKeys(int businessmanUserId)
        {
            return Context.Set<BusinessmanUserTrustedDevice>()
                          .Where(a => a.BusinessmanUserId == businessmanUserId
                                 && a.DateOfExpire > DateTime.Now)
                          .Select(a => a.UniqueKey)
                          .ToList();
        }

        public void CreateUserTrustedDeviceKeys(int businessmanUserId, string uniqueKey, DateTime dateOfExpire, string userIp, string userAgent)
        {
            var entity = new BusinessmanUserTrustedDevice
            {
                BusinessmanUserId = businessmanUserId,
                UniqueKey = uniqueKey,
                DateOfExpire = dateOfExpire,
                IpAdress = userIp,
                UserAgent = userAgent,
                LastAccessTime = DateTime.Now,
            };
            Context.Set<BusinessmanUserTrustedDevice>().Add(entity);
            Context.Entry(entity).State = EntityState.Added;
        }

        public BusinessmanUser Registrate(RegistrateBusinessmanUserDlDto dto, SmsCode smsCode)
        {
            var entity = dto.CreateEntity();
            entity.PasswordHash = smsCode.PasswordHash;
            entity.PasswordSalt = smsCode.PasswordSalt;
            entity.UserName = smsCode.PhoneNumber;
            DbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Added;

            return entity;
        }

        public void ChangeUserInfo(int id, UpdateUserInfoDlDto dto)
        {
            var entity = ById(id);
            dto.UpdateEntity(entity);
        }

        public void CreateBusinessmanUserContractorLog(BusinessmanUserContractorLogDlDto dto)
        {
            var entity = dto.CreateEntity();
            if(entity is null) { AddError("Entity is null !"); return; }

            Context.Set<BusinessmanUserContractorLog>().Add(entity);
            Context.Entry(entity).State = EntityState.Added;
        }

        public bool IsUserContractorPairActive(int businessmanUserId, long contractorId)
        {
            return Context.Set<BusinessmanUserInContractor>()
                          .Any(a => a.BusinessmanUserId == businessmanUserId
                                && a.StateId != StateIdConst.PASSIVE
                                && a.ContractorId == contractorId);
        }

        public BusinessmanUserContractorLog GetByUnique(string uniqueKey)
        {
            var now = DateTime.Now;
            return Context.Set<BusinessmanUserContractorLog>()
                .FirstOrDefault(x => x.UniqueKey == uniqueKey && x.DateOfExpire > now);
        }

        public void UpdateContractorId(string uniqueKey, long contractorId)
        {
            var entity = Context.Set<BusinessmanUserContractorLog>()
                .FirstOrDefault(x => x.UniqueKey == uniqueKey);

            if(entity is null)
            {
                AddError("Entity is null !");
                return;
            }

            entity.ContractorId = contractorId;
            Context.Entry(entity).State = EntityState.Modified;
        }

        protected override IQueryable<BusinessmanUser> InjectFilter(IQueryable<BusinessmanUser> query)
        {
            query = query.Where(x => x.StateId != StateIdConst.PASSIVE);

            return query;
        }
    }
}
