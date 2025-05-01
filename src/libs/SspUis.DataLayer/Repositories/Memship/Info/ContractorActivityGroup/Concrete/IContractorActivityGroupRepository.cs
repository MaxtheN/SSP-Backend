using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IContractorActivityGroupRepository : IBaseEntityRepository<int, ContractorActivityGroup, CreateContractorActivityGroupDlDto, UpdateContractorActivityGroupDlDto>
{
}
