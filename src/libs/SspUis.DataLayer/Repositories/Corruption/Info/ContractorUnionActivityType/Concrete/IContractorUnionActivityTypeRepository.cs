using SspUis.DataLayer.EfClasses.Corruption;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorUnionActivityTypeRepository : IBaseEntityRepository<int, ContractorUnionActivityType, CreateContractorUnionActivityTypeDlDto, UpdateContractorUnionActivityTypeDlDto>
{
}
