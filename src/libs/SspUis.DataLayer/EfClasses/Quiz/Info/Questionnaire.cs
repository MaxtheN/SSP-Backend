using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_questionnaire", Schema = "quiz")]
public partial class Questionnaire : IHaveIdProp<long>, IHaveStateId
{
    public Questionnaire()
    {
        Translates = new HashSet<QuestionnaireTranslate>();
        QuestionnaireGroups = new HashSet<QuestionnaireGroup>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("order_number")]
    public int? OrderNumber { get; set; }
    [Required]
    [Column("title")]
    [StringLength(500)]
    public string Title { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("details")]
    [StringLength(1024)]
    public string Details { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    [Column("questionnaire_type_id")]
    public int QuestionnaireTypeId { get; set; }

    [ForeignKey(nameof(QuestionnaireTypeId))]
    public virtual QuestionnaireType QuestionnaireType { get; set; }

    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(QuestionnaireTranslate.Owner))]
    public virtual ICollection<QuestionnaireTranslate> Translates { get; set; }
    [InverseProperty(nameof(QuestionnaireGroup.Owner))]
    public virtual ICollection<QuestionnaireGroup> QuestionnaireGroups { get; set; }
}
