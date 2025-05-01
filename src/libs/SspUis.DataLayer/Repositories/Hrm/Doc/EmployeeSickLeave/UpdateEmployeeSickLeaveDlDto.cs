using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateEmployeeSickLeaveDlDto : EmployeeSickLeaveDlDto<UpdateEmployeeSickLeaveDlDto>, IHaveIdProp<long>
{
    public long Id { get; set; }
}
