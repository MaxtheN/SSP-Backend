using SspUis.DataLayer.EfClasses.Appeal;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Appeal;

public class UpdateStatusAppealApplicationDlDto
    : EntityDto<UpdateStatusAppealApplicationDlDto, AppealApplication>
    ,IHaveIdProp<long>
    ,IHaveStatusId
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    //[LocalizedRequired]
    public int StatusId { get; set; }
    public string? Message { get; set; }
}
