using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MediationFileDlDto : EntityDto<MediationFileDlDto, MediationFile>, ILinkToEntity<MediationFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
