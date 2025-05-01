using SspUis.DataLayer.EfClasses.Kpi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_unite_of_measure", Schema = "public")]
public  class UniteOfMeasure : IHaveIdProp<int>, IHaveStateId
{
    public UniteOfMeasure()
    {
        
        Translates = new HashSet<UniteOfMeasureTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
	[Required]
	[Column("state_id")]
	public int StateId { get; set; }
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
	[ForeignKey(nameof(StateId))]
	public virtual State State { get; set; }
	[InverseProperty(nameof(UniteOfMeasureTranslate.Owner))]
    public virtual ICollection<UniteOfMeasureTranslate> Translates { get; set; }

   
}
