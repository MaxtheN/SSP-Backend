using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeMissedDayTableDlDto : EntityDto<EmployeeMissedDayTableDlDto, EmployeeMissedDayTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public long EmployeeManageId { get; set; }
    [LocalizedRequired]
    public int MissedDaysTypeId { get; set; }
    [LocalizedRequired]
    public int MissedDays { get; set; }
    [LocalizedRequired]
    public DateTime StartAt { get; set; }
    [LocalizedRequired]
    public DateTime EndAt { get; set; }
    [LocalizedRequired]
    public bool WithoutReason { get; set; }
    public string Details { get; set; }
}
