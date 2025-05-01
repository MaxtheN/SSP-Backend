using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;
public class AnswerDlDto<TDto> : EntityDto<TDto, Answer>
        where TDto : AnswerDlDto<TDto>
{
    public int? OrderNumber { get; set; }
    [LocalizedRequired]
    public string AnswerText { get; set; }
    public List<AnswerTranslateDlDto> Translates { get; set; } = new();
    protected override Action<IMappingExpression<TDto, Answer>> AlterMapping => cfg => cfg
        .ForMember(x => x.Translates, x => x.Ignore());
    public override Answer CreateEntity()
    {
        var entity = base.CreateEntity();
        entity.StateId = StateIdConst.ACTIVE;
        Translates.AddByUniqueFKTo(entity.Translates);
        return entity;
    }
    public override void UpdateEntity(Answer entity)
    {
        base.UpdateEntity(entity);
        Translates.ApplyChangesByUniqueFKTo(entity.Translates);
    }
}
