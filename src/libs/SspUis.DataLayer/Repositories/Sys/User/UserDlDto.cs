using SspUis.DataLayer.EfClasses;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.DataLayer.Repositories
{
    public class UserDlDto<TDto> : EntityDto<TDto, User>
        where TDto : UserDlDto<TDto>
    {
        public virtual string Password { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int OrganizationId { get; set; }
        //public int? EmployeeId { get; set; }
        public int? LanguageId { get; set; }
        [LocalizedEmailAddress]
        [LocalizedStringLength(250)]
        public string Email { get; set; }
        [LocalizedStringLength(50)]
        [LocalizedRequired]
        public string PhoneNumber { get; set; }
        public List<int> Roles { get; set; } = new List<int>();
        public long? EmployeeManageId { get; set; }
        public long? DocumentId { get; set; }

        public override User CreateEntity()
        {
            if (Email.NullOrEmpty())
                Email = null;
            var entity = base.CreateEntity();
            SetPassword(entity, true);
            entity.StateId = StateIdConst.ACTIVE;
            entity.PhoneNumber = new string(PhoneNumber.Where(char.IsDigit).ToArray());
            entity.UserRoles.AddFromForeignKeys(Roles);
            return entity;
        }

        public override void UpdateEntity(User entity)
        {
            if (Email.NullOrEmpty())
                Email = null;
            entity.UserRoles.UpdateByStateIdFromForeignKeys(Roles);
            base.UpdateEntity(entity);
            SetPassword(entity);
        }

        private void SetPassword(User entity, bool isNewEntity = false)
        {
            if (isNewEntity || !Password.NullOrEmpty())
            {
                entity.PasswordSalt = HashHelper.CreateRandomSalt();
                entity.PasswordHash = new CustomPaswordHasher().HashPassword(Password, entity.PasswordSalt);
            }
        }
    }
}
