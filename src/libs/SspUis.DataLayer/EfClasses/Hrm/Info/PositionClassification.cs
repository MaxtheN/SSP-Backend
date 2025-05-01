using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_position_classification", Schema = "hrm")]
public partial class PositionClassification : IHaveIdProp<int>, IHaveStateId
{
    public PositionClassification()
    {
        Translates = new HashSet<PositionClassificationTranslate>();
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
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Required]
    [Column("pn_ru")]
    [StringLength(500)]
    public string PnRu { get; set; }
    [Required]
    [Column("num_ru")]
    [StringLength(500)]
    public string NumRu { get; set; }
    [Required]
    [Column("class_ru")]
    [StringLength(500)]
    public string ClassRu { get; set; }
    [Required]
    [Column("nskz_code_ru")]
    [StringLength(500)]
    public string NskzCodeRu { get; set; }
    [Required]
    [Column("category_code_ru")]
    [StringLength(500)]
    public string CategoryCodeRu { get; set; }
    [Required]
    [Column("range_code_ru")]
    [StringLength(500)]
    public string RangeCodeRu { get; set; }
    [Required]
    [Column("mined_code_ru")]
    [StringLength(500)]
    public string MinedCodeRu { get; set; }
    [Required]
    [Column("speciality_code_ru")]
    [StringLength(500)]
    public string SpecialityCodeRu { get; set; }
    [Required]
    [Column("type_code_ru")]
    [StringLength(500)]
    public string TypeCodeRu { get; set; }
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

    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(PositionClassificationTranslate.Owner))]
    public virtual ICollection<PositionClassificationTranslate> Translates { get; set; }
}
