using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Claim;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_certificate", Schema = "memship")]
    [Index(nameof(Id2), Name = "doc_memship_certificate_id2_key", IsUnique = true)]
    public partial class MemshipCertificate : IHaveIdProp<long>, IHaveStatusId
    {
        public MemshipCertificate()
        {
            ClaimApplications = new HashSet<ClaimApplication>();
            Files = new HashSet<MemshipCertificateFile>();
        }
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
        [Column("memship_contract_id")]
        public long MemshipContractId { get; set; }
        [Column("expire_on")]
        public DateOnly ExpireOn { get; set; }
		[Column("details")]
		[StringLength(500)]
		public string? Details { get; set; }

		[Column("stat_id")]
		public int? StatId { get; set; }
		[Column("cancel_on")]
        public DateOnly? CancelOn { get; set; }
        [Column("cancel_day")]
        public DateOnly? CancelDay { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [Column("cancel_reasen")]
        public string CancelReasen { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("contractor_settlement_account_id")]
        public long? ContractorSettlementAccountId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(ContractorSettlementAccountId))]
        public virtual ContractorSettlementAccount ContractorSettlementAccount { get; set; }
        [ForeignKey(nameof(MemshipContractId))]
        public virtual MemshipContract MemshipContract { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(ClaimApplication.MemshipCertificate))]
        public virtual ICollection<ClaimApplication> ClaimApplications { get; set; }
        [InverseProperty(nameof(MemshipCertificateFile.Owner))]
        public virtual ICollection<MemshipCertificateFile> Files { get; set; }
    }
}