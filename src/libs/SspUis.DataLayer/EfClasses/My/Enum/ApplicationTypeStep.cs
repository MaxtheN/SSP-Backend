using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_application_type_step", Schema = "my")]
public partial class ApplicationTypeStep : IHaveIdProp<int>, IHaveStateId
{
    public ApplicationTypeStep()
    {
        Translates = new HashSet<ApplicationTypeStepTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("application_type_id")]
    public int ApplicationTypeId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ApplicationTypeId))]
    public virtual ApplicationType ApplicationType { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }

    [InverseProperty(nameof(ApplicationTypeStepTranslate.Owner))]
    public virtual ICollection<ApplicationTypeStepTranslate> Translates { get; set; }
}
