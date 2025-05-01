using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_contract", Schema = "memship")]
    [Index(nameof(Id2), Name = "uc_id2", IsUnique = true)]
    public partial class MemshipContract : IHaveIdProp<long>, IHaveStatusId
    {
        public MemshipContract()
        {
            Signs = new HashSet<MemshipContractSign>();
            Certificates = new HashSet<MemshipCertificate>();
            Files = new HashSet<MemshipContractFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("id2")]
        public Guid Id2 { get; set; }
        [Column("application_id")]
        public long? ApplicationId { get; set; }
        [Column("memship_contract_type_id")]
        public int MemshipContractTypeId { get; set; }
        [Column("contractor_category_id")]
        public int? ContractorCategoryId { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("base_fixed_minimum_value")]
        public decimal BaseFixedMinimumValue { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("contractor_settlement_account_id")]
        public long? ContractorSettlementAccountId { get; set; }
        [Column("organization_settlement_account_id")]
        public long? OrganizationSettlementAccountId { get; set; }
        [Column("message")]
        public string Message { get; set; }
		[Column("details")]
		[StringLength(500)]
		public string? Details { get; set; }
		[Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("regional_organization_id")]
        public int? RegionalOrganizationId { get; set; }
        [Column("region_id")]
        public int RegionId {  get; set; }
        [Column("district_id")]
        public int DistrictId {  get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }

        [Column("web_imzo_secret_key")]
        public string? WebImzoSecretKey { get; set; }
        [Column("web_imzo_request_id")]
        public Guid? WebImzoRequestId { get; set; }

        [Column("reject_message")]
        [StringLength (400)]
        public string? RejectMessage { get; set; }
        [Column("reject_date" , TypeName = "timestamp without time zone")]
        public DateTime? RejectDate { get; set; }

        [ForeignKey(nameof(ContractorCategoryId))]
        public virtual ContractorCategory ContractorCategory { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(ContractorSettlementAccountId))]
        public virtual ContractorSettlementAccount ContractorSettlementAccount { get; set; }
        [ForeignKey(nameof(MemshipContractTypeId))]
        public virtual MemshipContractType MemshipContractType { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RegionalOrganizationId))]
        public virtual Organization RegionalOrganization { get; set; }
        [ForeignKey(nameof(OrganizationSettlementAccountId))]
        public virtual OrganizationSettlementAccount OrganizationSettlementAccount { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(MemshipContractSign.Owner))]
        public virtual ICollection<MemshipContractSign> Signs { get; set; }
        [InverseProperty(nameof(MemshipCertificate.MemshipContract))]
        public virtual ICollection<MemshipCertificate> Certificates { get; set; }
        [InverseProperty(nameof(MemshipPaymentOrder.MemshipContract))]
        public virtual ICollection<MemshipPaymentOrder> PaymentOrders { get; set; }
        [InverseProperty(nameof(AdditionalAgreement.MemshipContract))]
        public virtual ICollection<AdditionalAgreement> AdditionalAgreements { get; set; }
        [InverseProperty(nameof(MemshipContractFile.Owner))]
        public virtual ICollection<MemshipContractFile> Files { get; set; }
    }
}
