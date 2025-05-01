using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Corruption
{
    [Table("doc_join_anti_corruption_certificate", Schema = "corruption")]
    public partial class JoinAntiCorruptionCertificate : IHaveIdProp<long>, IHaveStatusId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("id2")]
        public Guid Id2 { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("expire_on")]
        public DateOnly? ExpireOn { get; set; }
        [Column("cancel_on")]
        public DateOnly? CancelOn { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("anti_corruption_result_table_id")]
        public long? ResultId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(ResultId))]
        public virtual JoinAntiCorruptionResultTable JoinAntiCorruptionResultTable { get; set; }
    }
}
