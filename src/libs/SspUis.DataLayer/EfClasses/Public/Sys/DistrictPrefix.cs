using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Public.Sys;

[Table("sys_district_prefix", Schema ="public")]
public class DistrictPrefix
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("code")]
    [StringLength(4)]
    public string Code { get; set; }

    [Column("district_id")]
    public int DistrictId { get; set; }

    [Column("region_prefix_id")]
    public int RegionPrefixId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey("DistrictId")]
    public District District { get; set; }

    [ForeignKey("RegionPrefixId")]
    public RegionPrefix RegionPrefix { get; set; }
}
