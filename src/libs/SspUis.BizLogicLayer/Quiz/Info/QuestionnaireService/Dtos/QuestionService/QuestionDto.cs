using GenericServices;
using IhmaInv.BizLogicLayer.QuestionServices;
using SspUis.BizLogicLayer.AnswerService;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.QuestionService;

public class QuestionDto : UpdateQuestionDlDto, ILinkToEntity<Question>
{
    public string State { get; set; }
    public string AnswerType { get; set; }
    public string AnswerText { get; set; }
    new public List<QuestionTranslateDto> Translates { get; set; } = new();
    new public List<AnswerDto> Answers { get; set; } = new();
}
