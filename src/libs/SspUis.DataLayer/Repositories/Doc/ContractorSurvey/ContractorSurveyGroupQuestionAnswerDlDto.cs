using SspUis.DataLayer.EfClasses;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorSurveyGroupQuestionAnswerDlDto
        : EntityDto<ContractorSurveyGroupQuestionAnswerDlDto, ContractorSurveyGroupQuestionAnswers>,
        IHaveIdProp<long>
    {
        public long Id { get; set; }
        public long? QuestionnaireAnswerId { get; set; }
        public string TextAnswer { get; set; }

        public override void UpdateEntity(ContractorSurveyGroupQuestionAnswers entity)
        {
            base.UpdateEntity(entity);
        }
        public override ContractorSurveyGroupQuestionAnswers CreateEntity()
        {
            var ent = base.CreateEntity();
            return ent;
        }
    }
}
