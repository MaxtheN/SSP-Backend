using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories.Memship;

public interface IMemshipContractRepository : IBaseEntityRepository<long, MemshipContract, CreateMemshipContractDlDto, UpdateMemshipContractDlDto, UpdateStatusMemshipContractDlDto>
{
    (bool hasDocument, long documentId, long applicationId) IsThereMembershipContract(string inn);
    (bool hasDocument, long documentId) IsThereMembershipContractForBank(string inn);
}
