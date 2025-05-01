using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("enum_proposal_subject", Schema = "propos")]
    [Index(nameof(Code), Name = "uc_ps_code", IsUnique = true)]
    public partial class ProposalSubject
    {
        public ProposalSubject()
        {
            Proposals = new HashSet<Proposal>();
            Translates = new HashSet<ProposalSubjectTranslate>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [InverseProperty(nameof(Proposal.ProposalSubject))]
        public virtual ICollection<Proposal> Proposals { get; set; }
        [InverseProperty(nameof(ProposalSubjectTranslate.Owner))]
        public virtual ICollection<ProposalSubjectTranslate> Translates { get; set; }
    }
}