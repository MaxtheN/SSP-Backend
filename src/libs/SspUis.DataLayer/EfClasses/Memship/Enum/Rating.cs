using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;


namespace SspUis.DataLayer.EfClasses;

[Table("enum_rating", Schema = "memship")]
public partial class Rating
{
    public Rating()
    {
       Translates = new HashSet<RatingTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }

    [Column("maximum_percentage")]
    public decimal MaximumPercentage { get; set; }

    [Column("minimum_percentage")]
    public decimal MinimumPercentage { get; set; }

    [Required]
    [Column("full_name")]
    [StringLength(300)]
    public string FullName { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty(nameof(RatingTranslate.Owner))]
    public virtual ICollection<RatingTranslate> Translates { get; set; }
    
}