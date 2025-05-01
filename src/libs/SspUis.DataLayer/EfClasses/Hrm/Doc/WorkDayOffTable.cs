using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_work_day_off_table", Schema = "hrm")]
public partial class WorkDayOffTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("department_id")]
    public int DepartmentId { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("employee_manage_id")]
    public long? EmployeeManageId { get; set; }
    [Column("temp_employee_manage_id")]
    public long? TempEmployeeManageId { get; set; }
    [Column("day_off_on")]
    public DateOnly DayOffOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeManageId))]
    public virtual EmployeeManage EmployeeManage { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual WorkDayOff Owner { get; set; }
}
