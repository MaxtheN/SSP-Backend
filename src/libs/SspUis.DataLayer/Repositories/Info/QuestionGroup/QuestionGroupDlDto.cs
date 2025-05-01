using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class QuestionGroupDlDto<TDto> : EntityDto<TDto, QuestionGroup>
    where TDto : QuestionGroupDlDto<TDto>
{
    public int? OrderNumber { get; set; }
    [LocalizedRequired]
    public string Title { get; set; }
    public List<QuestionGroupTranslateDlDto> Translates { get; set; } = new();
    public List<UpdateQuestionDlDto> Questions { get; set; } = new();

    protected override Action<IMappingExpression<TDto, QuestionGroup>> AlterMapping => cfg => cfg
        .ForMember(x => x.Translates, x => x.Ignore());

    public override QuestionGroup CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StateId = StateIdConst.ACTIVE;
        Translates.AddByUniqueFKTo(entity.Translates);

        return entity;
    }

    public override void UpdateEntity(QuestionGroup entity)
    {
        base.UpdateEntity(entity);
        Translates.ApplyChangesByUniqueFKTo(entity.Translates);
    }
}

