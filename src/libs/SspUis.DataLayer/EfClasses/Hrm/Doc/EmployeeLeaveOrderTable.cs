using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_employee_leave_order_table", Schema = "hrm")]
public partial class EmployeeLeaveOrderTable : IHaveIdProp<long>
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
    [Column("is_with_out_pay")]
    public bool IsWithOutPay { get; set; }
    [Column("is_conscription")]
    public bool IsConscription { get; set; }
    [Column("start_on")]
    public DateOnly StartOn { get; set; }
    [Column("end_on")]
    public DateOnly EndOn { get; set; }
    [Column("days")]
    public int Days { get; set; }
    [Column("add_pay_days")]
    public int AddPayDays { get; set; }
    [Column("detail_for_print")]
    public string DetailForPrint { get; set; }
    [Column("for_period_start_on")]
    public DateOnly? ForPeriodStartOn { get; set; }
    [Column("for_period_end_on")]
    public DateOnly? ForPeriodEndOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("temp_employee_manage_id")]
    public long? TempEmployeeManageId { get; set; }
    [Column("work_start_date")]
    public DateOnly? WorkStartDate { get; set; }
    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeManageId))]
    public virtual EmployeeManage EmployeeManage { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(EmployeeLeaveOrder.Tables))]
    public virtual EmployeeLeaveOrder Owner { get; set; }
}
