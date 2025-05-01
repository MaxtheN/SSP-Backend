using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateStatusEmployeeSickLeaveDlDto : EntityDto<UpdateStatusEmployeeSickLeaveDlDto, EmployeeSickLeave>, IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    public int StatusId { get; set; }
}
