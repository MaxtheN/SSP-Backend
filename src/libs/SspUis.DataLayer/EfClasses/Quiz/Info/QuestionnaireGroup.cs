using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_questionnaire_group", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(GroupId), Name = "ux_info_questionnaire_group__owner", IsUnique = true)]
public partial class QuestionnaireGroup : IHaveIdProp<long>, IHaveSingleUniqueForeignKey<int>, IHaveStateId
{
    public QuestionnaireGroup()
    {
        QuestionnaireQuestions = new HashSet<QuestionnaireQuestion>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("group_id")]
    public int GroupId { get; set; }
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

    [ForeignKey(nameof(GroupId))]
    [InverseProperty(nameof(QuestionGroup.QuestionnaireGroups))]
    public virtual QuestionGroup Group { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Questionnaire.QuestionnaireGroups))]
    public virtual Questionnaire Owner { get; set; }
    [InverseProperty(nameof(QuestionnaireQuestion.Owner))]
    public virtual ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    public object GetUniqueForeignKey() => GroupId;
    public void SetUniqueForeignKey(int foreignKey) => GroupId = foreignKey;

}
