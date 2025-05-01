using GenericServices;
using IhmaInv.BizLogicLayer.QuestionServices;
using SspUis.BizLogicLayer.QuestionGroupService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;
namespace SspUis.BizLogicLayer.QuestionnaireService;

public class QuestionnaireDto : UpdateQuestionnaireDlDto, ILinkToEntity<Questionnaire>
{
    public string State { get; set; }
    new public List<QuestionnaireTranslateDto> Translates { get; set; } = new();
    new public List<QuestionGroupDto> Group { get; set; } = new();
}