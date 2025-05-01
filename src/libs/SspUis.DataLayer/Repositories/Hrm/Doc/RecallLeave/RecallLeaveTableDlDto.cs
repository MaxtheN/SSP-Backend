using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class RecallLeaveTableDlDto : EntityDto<RecallLeaveTableDlDto, RecallLeaveTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    //[LocalizedRequired]
    //[LocalizedRange(1,int.MaxValue)]
    //public int DepartmentId { get; set; }
    //[LocalizedRequired]
    //[LocalizedRange(1,int.MaxValue)]
    //public int EmployeeId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,long.MaxValue)]
    public long EmployeeLeaveOrderId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,long.MaxValue)]
    public long EmployeeLeaveOrderTableId { get; set; }
    [LocalizedRequired]
    public DateOnly StartOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    [LocalizedRequired]
    public DateOnly? WorkStartDate { get; set; }
}
