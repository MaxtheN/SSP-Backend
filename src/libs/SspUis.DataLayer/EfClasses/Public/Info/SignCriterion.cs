using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_sign_criterion", Schema = "public")]
public class SignCriterion : IHaveIdProp<int>, IHaveStateId
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("date_on")]
    public DateTime DateOn { get; set; }
    [Required]
    [Column("state_id")]
    public int StateId { get; set; }
    [Required]
    [Column("position_id")]
    public int PositionId { get; set; }
    [Required]
    [Column("contractor_category_id")]
    public int ContractorCategoryId { get; set; }
    [Required]
    [Column("organization_group_id")]
    public int OrganizationGroupId { get; set; }
    [Required]
    [Column("application_type_id")]
    public int ApplicationTypeId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }
    [ForeignKey(nameof(ContractorCategoryId))]
    public virtual ContractorCategory ContractorCategory { get; set; }
    [ForeignKey(nameof(OrganizationGroupId))]
    public virtual OrganizationGroup OrganizationGroup { get; set; }
    [ForeignKey(nameof(ApplicationTypeId))]
    public virtual ApplicationType ApplicationType { get; set; }
}
