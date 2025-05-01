using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Public.Sys;

[Table("sys_region_prefix", Schema = "public")]
public class RegionPrefix
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("prefix")]
    [StringLength(255)]
    public string Prefix { get; set; }

    [Column("region_id")]
    public int RegionId { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }

    [Column("modified_at")]
    public DateTime? ModifiedAt { get; set; }

    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey("RegionId")]
    public Region Region { get; set; }
}
