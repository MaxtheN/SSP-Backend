using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("hl_department", Schema = "hrm")]
public partial class Department :IHaveIdProp<int> , IHaveStateId
{
    public Department()
    {
        Translates = new HashSet<DepartmentTranslate>();
        InverseParent = new HashSet<Department>();
    }
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string? OrderCode { get; set; }
    [Column("code")]
    public long? Code { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("parent_id")]
    public int? ParentId { get; set; }
    [Column("index_code")]
    [StringLength(5)]
    public string? IndexCode { get; set; }
    [Column("indicator_department_id")]
    public int? IndicatorDepartmentId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(IndicatorDepartmentId))]
    public virtual IndicatorDepartment IndicatorDepartment { get; set; }
    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(Department.InverseParent))]
    public virtual Department Parent { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(Department.Parent))]
    public virtual ICollection<Department> InverseParent { get; set; }
    [InverseProperty(nameof(DepartmentTranslate.Owner))]
    public virtual ICollection<DepartmentTranslate> Translates { get; set; }
}
