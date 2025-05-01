using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class UpdateStatusContractorSurveyDlDto : EntityDto<UpdateStatusContractorSurveyDlDto, ContractorSurvey>
{
    [LocalizedRequired]
    [LocalizedRange(0, long.MaxValue)]
    public long Id { get; set; }
    [LocalizedRequired]
    public int StatusId { get; set; }
}
