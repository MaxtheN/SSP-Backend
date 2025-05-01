using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ChangePasswordDlDto : EntityDto<ChangePasswordDlDto, User>
    {
        [LocalizedRequired]
        public string CurrentPassword { get; set; }

        [LocalizedRequired]
        public string NewPassword { get; set; }
        [LocalizedRequired]
        [LocalizedCompare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }

        public override void UpdateEntity(User entity)
        {
            entity.SetPassword(NewPassword);
        }
    }
}
