using SspUis.DataLayer.EfClasses.Quiz.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_question", Schema = "quiz")]
    public partial class Question : IHaveIdProp<int>, IHaveStateId
    {
        public Question()
        {
            Translates = new HashSet<QuestionTranslate>();
            QuestionnaireQuestions = new HashSet<QuestionnaireQuestion>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_number")]
        public int? OrderNumber { get; set; }
        [Required]
        [Column("question_text")]
        public string QuestionText { get; set; }
        [Column("hint")]
        [StringLength(500)]
        public string Hint { get; set; }
        [Column("answer_type_id")]
        public int AnswerTypeId { get; set; }
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

        [ForeignKey(nameof(AnswerTypeId))]
        public virtual AnswerType AnswerType { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(QuestionTranslate.Owner))]
        public virtual ICollection<QuestionTranslate> Translates { get; set; }
        [InverseProperty(nameof(QuestionnaireQuestion.Question))]
        public virtual ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    }
}
