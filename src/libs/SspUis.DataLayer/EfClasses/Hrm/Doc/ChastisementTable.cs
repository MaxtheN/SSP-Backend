using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_chastisement_table", Schema = "hrm")]
public partial class ChastisementTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("employee_manage_id")]
    public long EmployeeManageId { get; set; }
    [Column("department_id")]
    public int DepartmentId { get; set; }
    [Column("employee_id")]
    public int EmployeeId { get; set; }
    [Column("position_id")]
    public int PositionId { get; set; }
    [Column("employee_rate")]
    [Precision(18, 2)]
    public decimal EmployeeRate { get; set; }
    [Column("detail_for_print")]
    public string DetailForPrint { get; set; }
    [Column("detail_print")]
    public string? DetailPrint { get; set; }
    [Column("has_reprimand")]
    public bool HasReprimand { get; set; }
    [Column("has_penalty")]
    public bool HasPenalty { get; set; }
    [Column("is_blocked")]
    public bool IsBlocked { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public virtual Employee Employee { get; set; }
    [ForeignKey(nameof(EmployeeManageId))]
    public virtual EmployeeManage EmployeeManage { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Chastisement.Tables))]
    public virtual Chastisement Owner { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }
}
