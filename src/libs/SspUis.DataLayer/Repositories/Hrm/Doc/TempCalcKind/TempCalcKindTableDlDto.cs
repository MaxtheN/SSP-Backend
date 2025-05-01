using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class TempCalcKindTableDlDto : EntityDto<TempCalcKindTableDlDto, TempCalcKindTable>,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1,long.MaxValue)]
    public long EmployeeManageId { get; set; }
    [LocalizedRequired]
    public decimal Percentage { get; set; }
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    public string? DetailForPrint { get; set; }
}
