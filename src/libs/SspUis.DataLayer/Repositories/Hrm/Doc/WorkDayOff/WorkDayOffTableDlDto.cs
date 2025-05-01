using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class WorkDayOffTableDlDto : EntityDto<WorkDayOffTableDlDto, WorkDayOffTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    public long EmployeeManageId { get; set; }
    [LocalizedRequired]
    public DateOnly DayOffOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
}
