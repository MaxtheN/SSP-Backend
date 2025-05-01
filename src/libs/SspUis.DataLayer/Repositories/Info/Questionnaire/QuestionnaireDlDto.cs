using AutoMapper;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using WEBASE.EF;
namespace SspUis.DataLayer.Repositories;

public class QuestionnaireDlDto<TDto> : EntityDto<TDto, Questionnaire>
    where TDto : QuestionnaireDlDto<TDto>
{
    public int? OrderNumber { get; set; }
    public string Title { get; set; }
    public string Details { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int QuestionnaireTypeId { get; set; }
    public List<QuestionnaireTranslateDlDto> Translates { get; set; } = new();
    public List<UpdateQuestionGroupDlDto> Group { get; set; } = new();

    [JsonIgnore]
    public List<QuestionnaireGroupDlDto> GroupsOnlyIds { get; set; } = new();

    protected override Action<IMappingExpression<TDto, Questionnaire>> AlterMapping => cfg => cfg
        .ForMember(x => x.Translates, x => x.Ignore())
        .ForMember(x => x.QuestionnaireGroups, x => x.Ignore());

    public override Questionnaire CreateEntity()
    {
        var entity = base.CreateEntity();
        Translates.AddByUniqueFKTo(entity.Translates);
        GroupsOnlyIds.AddTo(entity.QuestionnaireGroups);

        return entity;
    }

    public override void UpdateEntity(Questionnaire entity)
    {
        base.UpdateEntity(entity);
        Translates.ApplyChangesByUniqueFKTo(entity.Translates);
        GroupsOnlyIds.ApplyChangesTo<long, QuestionnaireGroupDlDto, QuestionnaireGroup>(entity.QuestionnaireGroups);
    }
}