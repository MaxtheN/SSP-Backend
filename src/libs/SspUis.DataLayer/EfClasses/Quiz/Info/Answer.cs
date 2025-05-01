using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_answer", Schema = "quiz")]
public partial class Answer : IHaveStateId, IHaveIdProp<long>
{
    public Answer()
    {
        Translates = new HashSet<AnswerTranslate>();
        QuestionnaireAnswers = new HashSet<QuestionnaireAnswer>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("order_number")]
    public int? OrderNumber { get; set; }
    [Required]
    [Column("answer_text")]
    public string AnswerText { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("parent_id")]
    public long? ParentId { get; set; }
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
    [InverseProperty(nameof(AnswerTranslate.Owner))]
    public virtual ICollection<AnswerTranslate> Translates { get; set; }
    [InverseProperty(nameof(QuestionnaireAnswer.Answer))]
    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
}
