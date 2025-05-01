using SspUis.DataLayer.Repositories;
using StatusGeneric;
using WEBASE.Models;

namespace WbCrm.BizLogicLayer.ContractorSurveyServices;

public interface IContractorSurveyService : IStatusGeneric
{
    PagedResult<ContractorSurveyListDto> GetList(ContractorSurveySortFilterPageOptions options);
    ContractorSurveyDto Get();
    ContractorSurveyDto Get(long id);
    HaveId<long> Create(CreateContractorSurveyDlDto dto);
    HaveId<long> Delete(long id);
}
