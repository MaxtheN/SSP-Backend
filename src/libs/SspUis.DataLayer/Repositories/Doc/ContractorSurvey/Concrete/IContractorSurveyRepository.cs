using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorSurveyRepository : IBaseEntityRepository<long, ContractorSurvey, CreateContractorSurveyDlDto, UpdateContractorSurveyDlDto>
{
    ContractorSurvey UpdateStatus(UpdateStatusContractorSurveyDlDto dto);
}
