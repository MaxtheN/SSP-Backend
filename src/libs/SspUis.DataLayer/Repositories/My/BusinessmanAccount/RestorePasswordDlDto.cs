using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class RestorePasswordDlDto : EntityDto<BusinessmanChangePasswordDlDto, BusinessmanUser>
    {   
        [Required]
        public string UserName { get; set; }
        public string SmsCode { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string ConfirmedPassword { get; set; }

        public override void UpdateEntity(BusinessmanUser entity)
        {
            entity.SetPassword(NewPassword);
        }
    }
}
