using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contractor_contact", Schema = "my")]
    public partial class ContractorContact : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("contact")]
        [StringLength(500)]
        public string Contact { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("contact_type_id")]
        public int ContactTypeId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ContactTypeId))]
        public virtual ContactType ContactType { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Contractor.Contacts))]
        public virtual Contractor Owner { get; set; }
    }
}
