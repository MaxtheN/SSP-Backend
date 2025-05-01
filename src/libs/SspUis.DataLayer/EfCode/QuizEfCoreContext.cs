using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.EfCode;

public partial class EfCoreContext
{
    #region INFO
    public virtual DbSet<Questionnaire> Questionnaires { get; set; }
    public virtual DbSet<QuestionGroup> QuestionGroups { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<QuestionnaireGroup> QuestionnaireGroups { get; set; }
    public virtual DbSet<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }
    public virtual DbSet<QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
    public virtual DbSet<ContractorSurvey> ContractorSurveys { get; set; }
    public virtual DbSet<ContractorSurveyGroup> ContractorSurveyGroups { get; set; }
    public virtual DbSet<ContractorSurveyGroupQuestion> ContractorSurveyGroupQuestions { get; set; }
    public virtual DbSet<ContractorSurveyGroupQuestionAnswers> ContractorSurveyGroupQuestionAnswers { get; set; }
    public virtual DbSet<ContractorSurveyTable> ContractorSurveyTables { get; set; }
    #endregion
    #region Translates
    public virtual DbSet<QuestionnaireTranslate> QuestionnaireTranslates { get; set; }
    public virtual DbSet<QuestionGroupTranslate> QuestionGroupTranslates { get; set; }
    public virtual DbSet<QuestionTranslate> QuestionTranslates { get; set; }
    public virtual DbSet<AnswerTranslate> AnswerTranslates { get; set; }
    #endregion
}
