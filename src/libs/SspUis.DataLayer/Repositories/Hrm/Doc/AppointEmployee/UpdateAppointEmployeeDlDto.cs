using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateAppointEmployeeDlDto : AppointEmployeeDlDto<UpdateAppointEmployeeDlDto>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public long Id { get; set; }
}
