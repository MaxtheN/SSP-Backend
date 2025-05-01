using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Linq;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public class QuestionnaireListDtoConfig : PerDtoConfig<QuestionnaireListDto, Questionnaire>
{
    public override Action<IMappingExpression<Questionnaire, QuestionnaireListDto>> AlterReadMapping =>
    cfg => cfg
          .ForMember(x => x.State, x => x.MapFrom(ent => ent.State.Translates.AsQueryable()
            .FirstOrDefault(StateTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
          .TranslateText ?? ent.State.FullName))
         .ForMember(x => x.QuestionnaireType, x => x.MapFrom(ent => ent.QuestionnaireType.Translates.AsQueryable()
                .FirstOrDefault(QuestionnaireTypeTranslate.GetExpr(TranslateColumn.full_name, ServiceProvider.CultureHelper.CurrentCulture.Id))
              .TranslateText ?? ent.QuestionnaireType.FullName));
}