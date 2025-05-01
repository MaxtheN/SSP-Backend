using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.QuestionnaireService;

public class QuestionnaireListDto : ILinkToEntity<Questionnaire>, IHaveIdProp<long>
{
    public long Id { get; set; }
    public int? OrderNumber { get; set; }
    public string Title { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
    public string Details { get; set; }
    public int QuestionnaireTypeId { get; set; }
    public string QuestionnaireType { get; set; }
}
