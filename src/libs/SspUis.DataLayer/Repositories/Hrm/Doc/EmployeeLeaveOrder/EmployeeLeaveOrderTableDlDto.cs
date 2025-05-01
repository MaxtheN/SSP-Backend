using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeLeaveOrderTableDlDto : EntityDto<EmployeeLeaveOrderTableDlDto, EmployeeLeaveOrderTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)] 
    public int DepartmentId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    public string DetailForPrint { get; set; }
    public bool IsWithOutPay { get; set; }
    public bool IsConscription { get; set; }
    public DateOnly StartOn { get; set; }
    public DateOnly EndOn { get; set; }
    public int Days { get; set; }
    public int AddPayDays { get; set; }
    public DateOnly? ForPeriodStartOn { get; set; }
    public DateOnly? ForPeriodEndOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public long? TempEmployeeManageId { get; set; }
    [LocalizedRequired]
    public DateOnly? WorkStartDate { get; set; }
}
