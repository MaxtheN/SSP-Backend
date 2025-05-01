using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("info_institute", Schema = "dual_edu")]
[Index(nameof(Code), Name = "info_institute_ux_code", IsUnique = true)]
public partial class Institute : IHaveIdProp<int>, IHaveStateId
{
    public Institute()
    {
        Translates = new HashSet<InstituteTranslate>();
        Specialties = new HashSet<Specialty>();
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
    [Required]
    [Column("inn")]
    [StringLength(9)]
    public string Inn { get; set; }
    [Column("region_id")]
    public int RegionId { get; set; }
    [Column("district_id")]
    public int DistrictId { get; set; }
    [Column("hemis_id")]
    public int HemisId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
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
    [ForeignKey(nameof(RegionId))]
    public virtual Region Region { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District District { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(InstituteTranslate.Owner))]
    public virtual ICollection<InstituteTranslate> Translates { get; set; }
    [InverseProperty(nameof(Specialty.Owner))]
    public virtual ICollection<Specialty> Specialties { get; set; }
}
