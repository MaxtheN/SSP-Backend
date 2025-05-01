using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IDualContractRepository
        : IBaseEntityRepository<long, DualContract, CreateDualContractDlDto, UpdateDualContractDlDto, UpdateStatusDualContractDlDto>
{
}
