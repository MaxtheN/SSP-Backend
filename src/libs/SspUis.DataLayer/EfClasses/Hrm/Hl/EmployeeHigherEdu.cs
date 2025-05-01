using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("hl_employee_higheredu", Schema = "hrm")]
//[Index(nameof(OwnerId), Name = "ix_hl_personhigheredu__person")]
public partial class EmployeeHigherEdu : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("specialty_id")]
    public int SpecialtyId { get; set; }
    [Column("institute_id")]
    public int? InstituteId { get; set; }
    [Required]
    [Column("institute_name")]
    [StringLength(2000)]
    public string InstituteName { get; set; }
    [Column("document_series")]
    [StringLength(10)]
    public string DocumentSeries { get; set; }
    [Required]
    [Column("document_number")]
    [StringLength(10)]
    public string DocumentNumber { get; set; }
    [Column("employee_higher_edu_degree_id")]
    public int EmployeeHigherEduDegreeId { get; set; }
    [Column("date_of_issue")]
    public DateOnly DateOfIssue { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual Employee Owner { get; set; }
    [ForeignKey(nameof(SpecialtyId))]
    public virtual Specialty Specialty { get; set; }
    [ForeignKey(nameof(InstituteId))]
    public virtual Institute Institute { get; set; }
    [ForeignKey(nameof(EmployeeHigherEduDegreeId))]
    public virtual EmployeeHigherEduDegree EmployeeHigherEduDegrees { get; set; }
}
