using GenericServices;
using SspUis.DataLayer.EfClasses.Claim;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ClaimApplicationFileDlDto : EntityDto<ClaimApplicationFileDlDto, ClaimApplicationFile>,
        IHaveIdProp<Guid>,
        ILinkToEntity<ClaimApplicationFile>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
