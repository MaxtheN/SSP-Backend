using System;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories.Hrm;

public class AppointEmployeeTableDlDto : EntityDto<AppointEmployeeTableDlDto, AppointEmployeeTable> ,IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    public DateOnly StartOn { get; set; }
    public DateOnly? EndOn { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmpAppointOrderTypeId { get; set; }
    public string? Details { get; set; }
    public int? DepartmentId { get; set; }
    public string DetailForPrint { get; set; }
    public int? PositionId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    public int? EmploymentTypeId { get; set; }
    public decimal? EmployeeRate { get; set; }
    public int? WorkScheduleId { get; set; }
    public int? FromDepartmentId { get; set; }
    public int? FromPositionId { get; set; }
    public long? FromEmployeeManageId { get; set; }
    public DateOnly? ProbationStartDate { get; set; }
    public DateOnly? ProbationEndDate { get; set; }
    public bool IsProbation { get; set; }
    public decimal? FromEmployeeRate { get; set; }
    public long? ChoosenEmployeeManageId { get; set; }
    public long? EmployeeManageId { get; set; }
    public bool Interm { get; set; }
    public bool Acting { get; set; }
}
