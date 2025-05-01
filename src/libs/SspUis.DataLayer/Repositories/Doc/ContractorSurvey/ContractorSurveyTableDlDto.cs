using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;


public class ContractorSurveyTableDlDto : EntityDto<ContractorSurveyTableDlDto, ContractorSurveyTable>, IHaveIdProp<long>
{
    public long Id { get; set; }

    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long QuestionnaireGroupId { get; set; }
    [LocalizedRequired]
    [LocalizedRange(1, long.MaxValue)]
    public long QuestionnaireQuestionId { get; set; }
    public long? QuestionnaireAnswerId { get; set; }
    public string TextAnswer { get; set; }
    public override ContractorSurveyTable CreateEntity()
    {
        var ent = base.CreateEntity();
        return ent;
    }
}

