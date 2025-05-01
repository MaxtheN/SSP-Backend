using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_contract", Schema = "srv")]
    [Index(nameof(ApplicationId), Name = "ucc_application_id", IsUnique = true)]
    [Index(nameof(DocNumber), Name = "uc_doc_number", IsUnique = true)]
    public class ServiceContract : IHaveIdProp<long>, IHaveStatusId
    {
        public ServiceContract()
        {
            Groups = new HashSet<ServiceContractGroup>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("id2")]
        public Guid Id2 { get; set; }

        [Required]
        [StringLength(30)]
        [Column("doc_number")]
        public string DocNumber { get; set; }

        [Required]
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }

        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [Required]
        [Column("contractor_id")]
        public long ContractorId { get; set; }

        [Required]
        [Column("application_id")]
        public long ApplicationId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("web_imzo_secret_key")]
        public string? WebImzoSecretKey { get; set; }
        [Column("web_imzo_request_id")]
        public Guid? WebImzoRequestId { get; set; }
        [Column("message")]
        public string? Message { get; set; }
        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }

        [InverseProperty(nameof(ServiceContractGroup.Owner))]
        public virtual ICollection<ServiceContractGroup> Groups { get; set; }

        [InverseProperty(nameof(CompletedService.ServiceContract))]
        public virtual ICollection<CompletedService> CompletedServices { get; set; }

        [InverseProperty(nameof(ServiceContractSign.Owner))]
        public virtual ICollection<ServiceContractSign> Signs { get; set; }
    }
}
