using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class PrtnCertificatesignDlDto : EntityDto<PrtnCertificatesignDlDto, PrtnCertificateSign>, ILinkToEntity<PrtnCertificateSign>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        public long Id { get; set; }
        [LocalizedRequired]
        public long OwnerId { get; set; }
        public Guid SignFile { get; set; }
        public Guid DataFile { get; set; }
        public string SignedUserInfo { get; set; }
        public DateTime? SignedAt { get; set; }
        public int? StatusId { get; set; }
    }
}
