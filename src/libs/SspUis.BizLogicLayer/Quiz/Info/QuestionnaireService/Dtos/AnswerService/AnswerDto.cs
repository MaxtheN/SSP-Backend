using GenericServices;
using IhmaInv.BizLogicLayer.AnswerServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.AnswerService;

public class AnswerDto : UpdateAnswerDlDto, ILinkToEntity<Answer>
{
    public string State { get; set; }
    public bool IsChecked { get; set; } = false;
    new public List<AnswerTranslateDto> Translates { get; set; } = new();
}
