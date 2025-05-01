using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateRecallLeaveDlDto : RecallLeaveDlDto<UpdateRecallLeaveDlDto> ,IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1,int.MaxValue)]
    public long Id { get; set; }
}
