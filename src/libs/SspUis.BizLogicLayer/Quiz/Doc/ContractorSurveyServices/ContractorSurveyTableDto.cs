using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public class ContractorSurveyTableDto : ContractorSurveyTableDlDto, ILinkToEntity<ContractorSurveyTable>
{
    public decimal? OrgCurrencyRate { get; set; }
    public string ContractorOrderClientName { get; internal set; }
    public int GroupId { get; set; }
    public int QuestionId { get; set; }
    public long AnswerId { get; set; }
}
