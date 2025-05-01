using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_indicator_table", Schema = "kpi")]
public class IndicatorTable : IHaveIdProp<int>
{
	[Key]
	[Column("id")]
	public int Id { get; set; }
	[Column("owner_id")]
	public int OwnerId { get; set; }
	[Required]
	[Column("code")]
	[StringLength(10)]
	public string Code { get; set; }
	[Required]
	[Column("short_name")]
	[StringLength(250)]
	public string ShortName { get; set; }
	[Required]
	[Column("full_name")]
	[StringLength(500)]
	public string FullName { get; set; }
	[Column("created_at", TypeName = "timestamp without time zone")]
	public DateTime CreatedAt { get; set; }
	[Column("created_user_id")]
	public int? CreatedUserId { get; set; }
	[Column("modified_at", TypeName = "timestamp without time zone")]
	public DateTime? ModifiedAt { get; set; }
	[Column("modified_user_id")]
	public int? ModifiedUserId { get; set; }

	[ForeignKey(nameof(OwnerId))]
	public virtual Indicator Owner { get; set; }
}
