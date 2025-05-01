using SspUis.DataLayer.EfClasses.Proposal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IProposalRepository : IBaseEntityRepository<long, Proposal, CreateProposalDlDto, UpdateProposalDlDto>
    {

    }
}
