using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_contractor_survey_group_question_answer", Schema = "quiz")]
    [Index(nameof(OwnerId), nameof(QuestionnaireAnswerId), Name = "ux_doc_contractor_survey_group_question_answer__owner_qa", IsUnique = true)]
    public class ContractorSurveyGroupQuestionAnswers : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("questionnaire_answer_id")]
        public long? QuestionnaireAnswerId { get; set; }

        [Column("text_answer")]
        public string TextAnswer { get; set; }

        [ForeignKey(nameof(QuestionnaireAnswerId))]
        public virtual QuestionnaireAnswer QuestionnaireAnswer { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorSurveyGroupQuestion.Answers))]
        public virtual ContractorSurveyGroupQuestion Owner { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
    }
}
