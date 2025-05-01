using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.QuestionGroupService;

public class QuestionGroupDtoConfig : PerDtoConfig<QuestionGroupDto, QuestionGroup>
{
    public override Action<IMappingExpression<QuestionGroup, QuestionGroupDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
                         .ForMember(x => x.Title, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(QuestionGroupTranslate.GetExpr(TranslateQuestionGroup.title, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.Title))
        ;
}
