using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_question_group", Schema = "quiz")]
public partial class QuestionGroup : IHaveIdProp<int>
{
    public QuestionGroup()
    {
        Translates = new HashSet<QuestionGroupTranslate>();
        QuestionnaireGroups = new HashSet<QuestionnaireGroup>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_number")]
    public int? OrderNumber { get; set; }
    [Required]
    [Column("title")]
    [StringLength(500)]
    public string Title { get; set; }
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
    [InverseProperty(nameof(QuestionGroupTranslate.Owner))]
    public virtual ICollection<QuestionGroupTranslate> Translates { get; set; }
    [InverseProperty(nameof(QuestionnaireGroup.Group))]
    public virtual ICollection<QuestionnaireGroup> QuestionnaireGroups { get; set; }
}
