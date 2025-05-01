using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using WEBASE.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_contractor_survey_group_question", Schema = "quiz")]
    [Index(nameof(OwnerId), nameof(QuestionnaireQuestionId), Name = "ux_doc_contractor_survey_group_question__owner_qa", IsUnique = true)]
    public class ContractorSurveyGroupQuestion : IHaveIdProp<long>
    {
        public ContractorSurveyGroupQuestion()
        {
            Answers = new HashSet<ContractorSurveyGroupQuestionAnswers>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("questionnaire_question_id")]
        public long QuestionnaireQuestionId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [ForeignKey(nameof(QuestionnaireQuestionId))]
        public virtual QuestionnaireQuestion QuestionnaireQuestion { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorSurveyGroup.Questions))]
        public virtual ContractorSurveyGroup Owner { get; set; }

        [InverseProperty(nameof(ContractorSurveyGroupQuestionAnswers.Owner))]
        public virtual ICollection<ContractorSurveyGroupQuestionAnswers> Answers { get; set; }
    }
}
