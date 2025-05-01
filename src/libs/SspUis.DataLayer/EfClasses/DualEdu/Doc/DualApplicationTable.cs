using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("doc_dual_application_table", Schema = "dual_edu")]
public partial class DualApplicationTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Required]
    [Column("order_number")]
    [StringLength(30)]
    public string OrderNumber { get; set; }
    [Column("position_classification_id")]
    public int PositionClassificationId { get; set; }
    [Column("institute_id")]
    public int InstituteId { get; set; }
    [Column("specialty_id")]
    public int SpecialtyId { get; set; }
    [Column("empty_positions_count")]
    public int EmptyPositionsCount { get; set; }
    [Required]
    [Column("details")]
    [StringLength(1000)]
    public string Details { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(InstituteId))]
    public virtual InstituteBilling Institute { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(DualApplication.Tables))]
    public virtual DualApplication Owner { get; set; }
    [ForeignKey(nameof(PositionClassificationId))]
    public virtual PositionClassification PositionClassification { get; set; }
    [ForeignKey(nameof(SpecialtyId))]
    public virtual SpecialtyBilling Specialty { get; set; }
}