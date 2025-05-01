using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_specialty", Schema = "dual_edu")]
[Index(nameof(Code), Name = "info_specialty_ux_code", IsUnique = true)]
public partial class Specialty : IHaveIdProp<int>, IHaveStateId
{
    public Specialty()
    {
        Translates = new HashSet<SpecialtyTranslate>();
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
    [Column("text_code")]
    [StringLength(50)]
    public string TextCode { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
	[Column("external_id")]
	public int ExternalId { get; set; }
	[Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual Institute Owner { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }

    [InverseProperty(nameof(SpecialtyTranslate.Owner))]
    public virtual ICollection<SpecialtyTranslate> Translates { get; set; }
}
