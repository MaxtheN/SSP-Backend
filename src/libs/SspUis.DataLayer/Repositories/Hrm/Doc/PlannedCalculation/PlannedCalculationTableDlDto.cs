using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class PlannedCalculationTableDlDto : EntityDto<PlannedCalculationTableDlDto, PlannedCalculationTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    public DateOnly StartOn { get; set; }
    public DateOnly? EndDate { get; set; }
    [LocalizedStringLength(600)]
    public string Details { get; set; }
    [LocalizedRequired]
    [LocalizedRange(0, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    public decimal Percentage { get; set; }
    public decimal Amount { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public int TempCalcKindTypeId { get; set; }
    public override PlannedCalculationTable CreateEntity()
    {
        var entity = base.CreateEntity();
        return entity;
    }

    public override void UpdateEntity(PlannedCalculationTable entity)
    {
        base.UpdateEntity(entity);
    }
}
