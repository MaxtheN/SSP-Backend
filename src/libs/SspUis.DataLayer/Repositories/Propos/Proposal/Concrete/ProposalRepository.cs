using GenericServices;
using SspUis.DataLayer.EfClasses.Proposal;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class ProposalRepository : BaseEntityRepository<long, Proposal, CreateProposalDlDto, UpdateProposalDlDto>, IProposalRepository
    {
        public ProposalRepository(ICrudServices crudServices)
            : base(crudServices)
        {

        }
    }
}
