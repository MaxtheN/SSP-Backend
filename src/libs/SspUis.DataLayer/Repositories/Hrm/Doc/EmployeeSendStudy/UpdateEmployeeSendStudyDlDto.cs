using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateEmployeeSendStudyDlDto : EmployeeSendStudyDlDto<UpdateEmployeeSendStudyDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
}
