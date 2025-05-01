using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class EmployeeSickLeaveTableDlDto : EntityDto<EmployeeSickLeaveTableDlDto, EmployeeSickLeaveTable>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(0, long.MaxValue)]
    public long EmployeeManageId { get; set; }
    [LocalizedRequired]
    public DateOnly GiveOn { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(5)]
    public string DocumentSeria { get; set; } 
    [LocalizedRequired]
    [LocalizedStringLength(10)]
    public string DocumentNumber { get; set; }
    public string DetailForPrint { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(300)]
    public string Diagnosis { get; set; } 
    [LocalizedRequired]
    [LocalizedStringLength(300)]
    public string GivenOrganization { get; set; } 
    [LocalizedRequired]
    public DateOnly StartOn { get; set; }
    [LocalizedRequired]
    public DateOnly EndOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int YearWorkExp { get; set; }
    [LocalizedRequired]
    public decimal CalcPerc { get; set; }
    [LocalizedRequired]
    public bool IsMaternityLeave { get; set; }
    [LocalizedStringLength(600)]
    public string? Details { get; set; }

    public override EmployeeSickLeaveTable CreateEntity()
    {
        var entity = base.CreateEntity();
        return entity;
    }

    public override void UpdateEntity(EmployeeSickLeaveTable entity)
    {
        base.UpdateEntity(entity);
    }
}
