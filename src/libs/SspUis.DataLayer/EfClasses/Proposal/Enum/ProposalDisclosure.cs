using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("enum_proposal_disclosure", Schema = "propos")]
    [Index(nameof(Code), Name = "uc_pd_code", IsUnique = true)]
    public partial class ProposalDisclosure
    {
        public ProposalDisclosure()
        {
            Translates = new HashSet<ProposalDisclosureTranslate>();
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
        [InverseProperty(nameof(ProposalDisclosureTranslate.Owner))]
        public virtual ICollection<ProposalDisclosureTranslate> Translates { get; set; }
    }
}
