using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class ContractorSurveyGroupDlDto
        : EntityDto<ContractorSurveyGroupDlDto, ContractorSurveyGroup>, IHaveIdProp<long>
    {
        public long Id { get; set; }

        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long QuestionnaireGroupId { get; set; }
        public List<ContractorSurveyGroupQuestionDlDto> Questions { get; set; } = new();
        protected override Action<IMappingExpression<ContractorSurveyGroupDlDto, ContractorSurveyGroup>> AlterMapping => cfg => cfg
        .ForMember(x => x.Questions, x => x.Ignore());

        public override ContractorSurveyGroup CreateEntity()
        {
            var ent = base.CreateEntity();
            Questions.AddTo(ent.Questions);
            return ent;
        }

        public override void UpdateEntity(ContractorSurveyGroup entity)
        {
            base.UpdateEntity(entity);
            Questions.ApplyChangesTo<long, ContractorSurveyGroupQuestionDlDto, ContractorSurveyGroupQuestion>(entity.Questions);
        }
    }
}
