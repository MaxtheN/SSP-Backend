using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_contractor_survey_group", Schema = "quiz")]
    [Index(nameof(OwnerId), nameof(QuestionnaireGroupId), Name = "ux_doc_contractor_survey_group__owner_qa", IsUnique = true)]
    public class ContractorSurveyGroup : IHaveIdProp<long>
    {
        public ContractorSurveyGroup()
        {
            Questions = new HashSet<ContractorSurveyGroupQuestion>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("owner_id")]
        public long OwnerId { get; set; }

        [Column("questionnaire_group_id")]
        public long QuestionnaireGroupId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ContractorSurvey.Groups))]
        public virtual ContractorSurvey Owner { get; set; }

        [ForeignKey(nameof(QuestionnaireGroupId))]
        public virtual QuestionnaireGroup QuestionnaireGroup { get; set; }

        [InverseProperty(nameof(ContractorSurveyGroupQuestion.Owner))]
        public virtual ICollection<ContractorSurveyGroupQuestion> Questions { get; set; }
    }
}
