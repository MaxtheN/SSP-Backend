using WEBASE.Models;
using WEBASE.EF;
using WEBASE.Attributes;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateStatusPrtnCertificateDlDto : EntityDto<UpdateStatusPrtnCertificateDlDto, PrtnCertificate>, IHaveIdProp<long>
    {
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        public string Message { get; set; } = null!;
        [LocalizedRequired]
        public int StatusId { get; set; }
    }
}
