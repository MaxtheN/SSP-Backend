using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorActivityTypeRepository : 
	IBaseEntityRepository<int, ContractorActivityType, 
		CreateContractorActivityTypeDlDto, UpdateContractorActivityTypeDlDto>
{
}
