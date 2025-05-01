using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class QuestionnaireGroupDlDto : EntityDto<QuestionnaireGroupDlDto, QuestionnaireGroup>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int GroupId { get; set; }
    public int StateId { get; set; }
    public List<QuestionnaireQuestionDlDto> Questions { get; set; } = new();

    protected override Action<IMappingExpression<QuestionnaireGroupDlDto, QuestionnaireGroup>> AlterMapping => cfg => cfg
        .ForMember(x => x.QuestionnaireQuestions, x => x.Ignore());

    public override QuestionnaireGroup CreateEntity()
    {
        var entity = base.CreateEntity();
        Questions.AddTo(entity.QuestionnaireQuestions);
        return entity;
    }

    public override void UpdateEntity(QuestionnaireGroup entity)
    {
        base.UpdateEntity(entity);
        Questions.ApplyChangesTo<long, QuestionnaireQuestionDlDto, QuestionnaireQuestion>(entity.QuestionnaireQuestions);
    }
}