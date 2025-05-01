using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_recall_leave_table", Schema = "hrm")]
public partial class RecallLeaveTable : IHaveIdProp<long>
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
    [Column("employee_leave_order_id")]
    public long EmployeeLeaveOrderId { get; set; }
    [Column("employee_leave_order_table_id")]
    public long EmployeeLeaveOrderTableId { get; set; }
    [Column("start_on")]
    public DateOnly StartOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }

    [Column("work_start_date")]
    public DateOnly? WorkStartDate { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeLeaveOrderId))]
    public virtual EmployeeLeaveOrder EmployeeLeaveOrder { get; set; }
    [ForeignKey(nameof(EmployeeLeaveOrderTableId))]
    public virtual EmployeeLeaveOrderTable EmployeeLeaveOrderTable { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(RecallLeave.Tables))]
    public virtual RecallLeave Owner { get; set; }
}
