using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorCategoryCriterionRepository : IBaseEntityRepository<long, ContractorCategoryCriterion, CreateContractorCategoryCriterionDlDto, UpdateContractorCategoryCriterionDlDto,UpdateStatusContractorCategoryCriterionDlDto>
{
}
