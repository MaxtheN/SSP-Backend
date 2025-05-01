using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("doc_proposal_attach_file", Schema = "propos")]
    public class ProposalFile : FileEntity<long>
    {
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Proposal.ProposalFiles))]
        public virtual Proposal Owner { get; set; }
    }
}
