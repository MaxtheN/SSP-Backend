using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class BusinessmanChangePhoneNumberDlDto : EntityDto<BusinessmanChangePhoneNumberDlDto, BusinessmanUser>
    {
        [LocalizedRequired]
        public string UserName { get; set; }

        public override void UpdateEntity(BusinessmanUser entity)
        {
            base.UpdateEntity(entity);
            entity.UserName = UserName;
        }
    }
}
