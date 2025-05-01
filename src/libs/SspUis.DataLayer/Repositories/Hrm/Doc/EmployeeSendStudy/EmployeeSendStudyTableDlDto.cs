using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSendStudyTableDlDto : EntityDto<EmployeeSendStudyTableDlDto, EmployeeSendStudyTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(2000)]
    public string University { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DepartmentId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    [LocalizedRequired]
    public DateOnly StartOn { get; set; }
    [LocalizedRequired]
    public DateOnly EndOn { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string DetailForPrint { get; set; }
    [LocalizedRequired]
    public DateOnly? WorkStartDate { get; set; }

}
