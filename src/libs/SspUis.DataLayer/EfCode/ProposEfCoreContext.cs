using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Proposal;
using WEBASE.EF;

namespace SspUis.DataLayer.EfCode
{
    public partial class EfCoreContext : BaseDbContext
    {
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<ProposalFile> ProposalFiles { get; set; }
        public DbSet<ProposalSubject> ProposalSubjects { get; set; }
        public DbSet<ProposalDisclosure> ProposalDisclosures { get; set; }
        public virtual DbSet<ProposalDisclosureTranslate> ProposalDisclosureTranslates { get; set; }
        public virtual DbSet<ProposalSubjectTranslate> ProposalSubjectTranslates { get; set; }
    }
}
