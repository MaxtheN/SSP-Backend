using SspUis.DataLayer.EfClasses;
using System;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class RegistrateBusinessmanUserDlDto : EntityDto<RegistrateBusinessmanUserDlDto, BusinessmanUser>
    {
        [LocalizedStringLength(250)]
        public string Email { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(500)]
        public string FullName { get; set; }
        //[LocalizedRequired]
        [LocalizedStringLength(50)]
        public string Inn { get; set; }
        [LocalizedStringLength(50)]
        public string Pinfl { get; set; }
        [LocalizedStringLength(100)]
        public string ESignCertificateNumber { get; set; }
        public Guid? SignDataFile { get; internal set; }
        public bool IsPinfl { get; set; }
        public override BusinessmanUser CreateEntity()
        {
            SignDataFile = Guid.NewGuid();
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            Email = string.IsNullOrWhiteSpace(Email) ? null : Email;

            return entity;
        }
    }
}
