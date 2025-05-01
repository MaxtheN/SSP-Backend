using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_appoint_employee_table", Schema = "hrm")]
public partial class AppointEmployeeTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("order_number")]
    public int OrderNumber { get; set; }
    [Column("start_on")]
    public DateOnly StartOn { get; set; }
    [Column("end_on")]
    public DateOnly? EndOn { get; set; }
    [Column("probation_start_date")]
    public DateOnly? ProbationStartDate { get; set; }
    [Column("probation_end_date")]
    public DateOnly? ProbationEndDate { get; set; }
    [Column("is_probation")]
    public bool IsProbation { get; set; }
    [Column("emp_appoint_order_type_id")]
    public int EmpAppointOrderTypeId { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
    [Column("department_id")]
    public int? DepartmentId { get; set; }
    [Column("position_id")]
    public int? PositionId { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("employment_type_id")]
    public int? EmploymentTypeId { get; set; }
    [Column("detail_for_print")]
    public string DetailForPrint { get; set; }
    [Column("employee_rate")]
    [Precision(18, 2)]
    public decimal? EmployeeRate { get; set; }
    [Column("work_schedule_id")]
    public int? WorkScheduleId { get; set; }
    [Column("employee_manage_id")]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Computed)]
    public long? EmployeeManageId { get; set; }
    [Column("from_department_id")]
    public int? FromDepartmentId { get; set; }
    [Column("from_position_id")]
    public int? FromPositionId { get; set; }
    [Column("from_employee_manage_id")]
    public long? FromEmployeeManageId { get; set; }
    [Column("choosen_employee_manage_id")]
    public long? ChoosenEmployeeManageId { get; set; }
    [Column("from_employee_rate")]
    [Precision(18, 2)]
    public decimal? FromEmployeeRate { get; set; }
    [Column("interim")]
    public bool Interm { get; set; }
    [Column("acting")]
    public bool Acting { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(EmpAppointOrderTypeId))]
    public virtual EmpAppointOrderType EmpAppointOrderType { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmploymentTypeId))]
    public virtual EmploymentType EmploymentType { get; set; }
    [ForeignKey(nameof(FromDepartmentId))]
    public virtual Department FromDepartment { get; set; }
    [ForeignKey(nameof(FromPositionId))]
    public virtual Position FromPosition { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual AppointEmployee Owner { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }
    [ForeignKey(nameof(WorkScheduleId))]
    public virtual WorkSchedule WorkSchedule { get; set; }
    //[ForeignKey(nameof(FromEmployeeManageId))]
    //public virtual EmployeeManage FromEmployeeManage { get; set; }
    [ForeignKey(nameof(ChoosenEmployeeManageId))]
    public virtual EmployeeManage ChoosenEmployeeManage { get; set; }
}
