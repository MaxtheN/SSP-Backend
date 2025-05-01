using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_questionnaire_question", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(QuestionId), Name = "ux_info_questionnaire_question__owner", IsUnique = true)]
public partial class QuestionnaireQuestion : IHaveIdProp<long>, IHaveSingleUniqueForeignKey<int>,IHaveStateId
{
    public QuestionnaireQuestion()
    {
        QuestionnaireAnswers = new HashSet<QuestionnaireAnswer>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("question_id")]
    public int QuestionId { get; set; }
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

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(QuestionnaireGroup.QuestionnaireQuestions))]
    public virtual QuestionnaireGroup Owner { get; set; }
    [ForeignKey(nameof(QuestionId))]
    public virtual Question Question { get; set; }
    [InverseProperty(nameof(QuestionnaireAnswer.Owner))]
    public virtual ICollection<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }

    public object GetUniqueForeignKey() => QuestionId;
    public void SetUniqueForeignKey(int foreignKey) => QuestionId = foreignKey;
}
