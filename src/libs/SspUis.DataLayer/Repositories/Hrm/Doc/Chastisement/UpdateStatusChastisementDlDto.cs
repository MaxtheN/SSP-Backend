using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class UpdateStatusChastisementDlDto
    : EntityDto<UpdateStatusChastisementDlDto, Chastisement>
    ,IHaveIdProp<long>
    ,IHaveStatusId
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int StatusId { get; set; }
}
