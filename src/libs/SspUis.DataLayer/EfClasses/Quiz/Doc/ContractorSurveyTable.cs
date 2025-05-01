using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_contractor_survey_table", Schema = "quiz")]
[Index(nameof(OwnerId), nameof(QuestionnaireGroupId), nameof(QuestionnaireQuestionId), nameof(QuestionnaireAnswerId), Name = "ux_doc_contractor_survey__owner_qa", IsUnique = true)]
public partial class ContractorSurveyTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("questionnaire_group_id")]
    public long QuestionnaireGroupId { get; set; }
    [Column("questionnaire_question_id")]
    public long QuestionnaireQuestionId { get; set; }
    [Column("questionnaire_answer_id")]
    public long? QuestionnaireAnswerId { get; set; }
    [Column("text_answer")]
    public string TextAnswer { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    //[ForeignKey(nameof(OwnerId))]
    //[InverseProperty(nameof(ContractorSurvey.Groups))]
    //public virtual ContractorSurvey Owner { get; set; }
    [ForeignKey(nameof(QuestionnaireAnswerId))]
    public virtual QuestionnaireAnswer QuestionnaireAnswer { get; set; }
    [ForeignKey(nameof(QuestionnaireGroupId))]
    public virtual QuestionnaireGroup QuestionnaireGroup { get; set; }
    [ForeignKey(nameof(QuestionnaireQuestionId))]
    public virtual QuestionnaireQuestion QuestionnaireQuestion { get; set; }
}
