using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Corruption;

public class UpdateJoinAntiCorruptionCertificateDlDto : JoinAntiCorruptionCertificateDlDto<UpdateJoinAntiCorruptionCertificateDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public long Id { get; set; }
}
