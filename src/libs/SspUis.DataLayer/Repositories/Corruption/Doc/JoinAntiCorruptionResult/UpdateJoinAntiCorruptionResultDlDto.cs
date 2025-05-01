using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Corruption;

public class UpdateJoinAntiCorruptionResultDlDto : JoinAntiCorruptionResultDlDto<UpdateJoinAntiCorruptionResultDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public long Id { get; set; }
}
