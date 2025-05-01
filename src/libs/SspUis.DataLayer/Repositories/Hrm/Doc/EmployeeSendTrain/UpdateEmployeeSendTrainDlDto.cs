using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateEmployeeSendTrainDlDto : EmployeeSendTrainDlDto<UpdateEmployeeSendTrainDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
}
