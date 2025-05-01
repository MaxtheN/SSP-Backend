using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_claim_application_table", Schema = "claim")]
    [Index(nameof(OwnerId), nameof(InnOrPinfl), Name = "ux_doc_claim_application_table__inn_pinfl", IsUnique = true)]
    public partial class ClaimApplicationTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Required]
        [Column("order_number")]
        [StringLength(30)]
        public string OrderNumber { get; set; }
        [Column("claim_responsible_type_id")]
        public int ClaimResponsibleTypeId { get; set; }
        [Required]
        [Column("inn_or_pinfl")]
        [StringLength(14)]
        public string InnOrPinfl { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Required]
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; }
        [Required]
        [Column("phone_number")]
        [StringLength(500)]
        public string PhoneNumber { get; set; }
        [Column("is_registred")]
        public bool? IsRegistred { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(ClaimResponsibleTypeId))]
        public virtual ClaimResponsibleType ClaimResponsibleType { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(ClaimApplication.Tables))]
        public virtual ClaimApplication Owner { get; set; }
    }
}
