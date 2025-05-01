using GenericServices;
using IhmaInv.BizLogicLayer.QuestionGroupService;
using SspUis.BizLogicLayer.QuestionService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.QuestionGroupService;

public class QuestionGroupDto : UpdateQuestionGroupDlDto, ILinkToEntity<QuestionGroup>
{
    new public List<QuestionDto> Questions { get; set; } = new();
    new public List<QuestionGroupTranslateDto> Translates { get; set; } = new();
    public string State { get; set; }
}
