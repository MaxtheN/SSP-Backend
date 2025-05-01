using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TempCalcKindSignerDlDto : EntityDto<TempCalcKindSignerDlDto, TempCalcKindSigner> ,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int SignOrder { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int DepartmentId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeManageId { get; set; }
    public bool IsHr { get; set; } 
    public bool IsDirector { get; set; } 
}
