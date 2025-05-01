using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class QuestionnaireQuestionDlDto : EntityDto<QuestionnaireQuestionDlDto, QuestionnaireQuestion>, IHaveIdProp<long>
{
    public long Id { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int QuestionId { get; set; }
    public List<QuestionnaireAnswerlDto> Answers { get; set; } = new();
    public bool IsDeleted { get; set; }
    protected override Action<IMappingExpression<QuestionnaireQuestionDlDto, QuestionnaireQuestion>> AlterMapping
        => cfg => cfg.ForMember(x => x.QuestionnaireAnswers, x => x.Ignore());

    public override QuestionnaireQuestion CreateEntity()
    {
        var entity = base.CreateEntity();
        Answers.AddTo(entity.QuestionnaireAnswers);
        return entity;
    }

    public override void UpdateEntity(QuestionnaireQuestion entity)
    {
        base.UpdateEntity(entity);
        Answers.ApplyChangesTo<long, QuestionnaireAnswerlDto, QuestionnaireAnswer>(entity.QuestionnaireAnswers);
    }
}