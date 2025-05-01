using WEBASE.Attributes;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateDepartmentDlDto: DepartmentDlDto<UpdateDepartmentDlDto> ,IHaveIdProp<int>
{
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int StateId { get; set; }
}
