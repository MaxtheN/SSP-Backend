using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses;

[Table("enum_inspection_type", Schema = "quiz")]
public partial class InspectionType
{
    public InspectionType()
    {
        ContractorSurveys = new HashSet<ContractorSurvey>();
        Translates = new HashSet<InspectionTypeTranslate>();
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
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(ContractorSurvey.InspectionType))]
    public virtual ICollection<ContractorSurvey> ContractorSurveys { get; set; }
    [InverseProperty(nameof(InspectionTypeTranslate.Owner))]
    public virtual ICollection<InspectionTypeTranslate> Translates { get; set; }
}
