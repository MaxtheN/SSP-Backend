using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorSurveyGroupQuestionDlDto
        : EntityDto<ContractorSurveyGroupQuestionDlDto, ContractorSurveyGroupQuestion>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public long QuestionnaireQuestionId { get; set; }
        public List<ContractorSurveyGroupQuestionAnswerDlDto> Answers { get; set; } = new();

        protected override Action<IMappingExpression<ContractorSurveyGroupQuestionDlDto, ContractorSurveyGroupQuestion>>
            AlterMapping => cfg => cfg.ForMember(x => x.Answers, x => x.Ignore());

        public override ContractorSurveyGroupQuestion CreateEntity()
        {
            var ent = base.CreateEntity();
            Answers.AddTo(ent.Answers);
            return ent;
        }

        public override void UpdateEntity(ContractorSurveyGroupQuestion entity)
        {
            base.UpdateEntity(entity);
            Answers.ApplyChangesTo<long, ContractorSurveyGroupQuestionAnswerDlDto, ContractorSurveyGroupQuestionAnswers>(entity.Answers);
        }
    }
}
