using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_questionnaire_answer", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(AnswerId), Name = "ux_info_questionnaire_answer__owner", IsUnique = true)]
public partial class QuestionnaireAnswer : IHaveIdProp<long>, IHaveSingleUniqueForeignKey<long>, IHaveStateId
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("answer_id")]
    public long AnswerId { get; set; }
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

    [ForeignKey(nameof(AnswerId))]
    public virtual Answer Answer { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(QuestionnaireQuestion.QuestionnaireAnswers))]
    public virtual QuestionnaireQuestion Owner { get; set; }
    public object GetUniqueForeignKey() => AnswerId;
    public void SetUniqueForeignKey(long foreignKey) => AnswerId = foreignKey;
}
