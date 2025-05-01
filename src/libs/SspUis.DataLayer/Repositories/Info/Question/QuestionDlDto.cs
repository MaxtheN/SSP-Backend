using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class QuestionDlDto<TDto> : EntityDto<TDto, Question>
    where TDto : QuestionDlDto<TDto>
{
    public int? OrderNumber { get; set; }
    [LocalizedRequired]
    public string QuestionText { get; set; }
    public string Hint { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, int.MaxValue)]
    public int AnswerTypeId { get; set; }
    public List<QuestionTranslateDlDto> Translates { get; set; } = new List<QuestionTranslateDlDto>();
    public List<UpdateAnswerDlDto> Answers { get; set; } = new();
    protected override Action<IMappingExpression<TDto, Question>> AlterMapping => cfg => cfg
        .ForMember(x => x.Translates, x => x.Ignore());
    public override Question CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StateId = StateIdConst.ACTIVE;
        Translates.AddByUniqueFKTo(entity.Translates);

        return entity;
    }
    public override void UpdateEntity(Question entity)
    {
        base.UpdateEntity(entity);
        Translates.ApplyChangesByUniqueFKTo(entity.Translates);
    }
}
