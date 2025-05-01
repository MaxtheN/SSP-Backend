using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class ChastisementTableDlDto : EntityDto<ChastisementTableDlDto, ChastisementTable>,IHaveIdProp<long>
{
    public long Id { get; set; }

    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    public string DetailForPrint { get; set; }
    public string? DetailPrint { get; set; }
    public bool HasReprimand { get; set; }
    public bool HasPenalty { get; set; }
}
