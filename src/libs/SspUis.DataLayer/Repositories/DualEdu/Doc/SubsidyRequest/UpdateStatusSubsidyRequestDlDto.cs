using SspUis.Core;
using SspUis.DataLayer.EfClasses.DualEdu;
using SspUis.DataLayer.Repositories.Appeal;
using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateStatusSubsidyRequestDlDto: SubsidyRequestDlDto<UpdateStatusSubsidyRequestDlDto>
    , IHaveIdProp<long>
{
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long Id { get; set; }
    public string Message { get; set; } = null!;
    public int StatusId { get; set; }
}
