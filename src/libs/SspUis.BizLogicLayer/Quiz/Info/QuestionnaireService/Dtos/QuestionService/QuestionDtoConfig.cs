using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Quiz.Enum;
using SspUis.DataLayer.Repositories;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.QuestionService;

public class QuestionDtoConfig : PerDtoConfig<QuestionDto, Question>
{
    public override Action<IMappingExpression<Question, QuestionDto>> AlterReadMapping =>
        cfg => cfg
            .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
                .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.State.FullName))
            .ForMember(x => x.AnswerType, x => x.MapFrom(ent => ent.AnswerType.Translates.AsQueryable()
                .FirstOrDefault(AnswerTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.AnswerType.FullName))
             .ForMember(x => x.QuestionText, x => x.MapFrom(ent => ent.Translates.AsQueryable()
                .FirstOrDefault(QuestionTranslate.GetExpr(TranslateQuestion.question_text, ServiceProvider.CultureHelper.CurrentCulture.Id)).TranslateText ?? ent.QuestionText))
            ;
}