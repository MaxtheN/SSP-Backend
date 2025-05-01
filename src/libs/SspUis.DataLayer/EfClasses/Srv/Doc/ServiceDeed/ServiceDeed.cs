using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_deed", Schema = "srv")]
    [Index(nameof(OrganizationId), nameof(StatusId), nameof(DocOn), Name = "ix_doc_service_deed_orgid_statusid_docon")]
    [Index(nameof(DocNumber), Name = "ucc_doc_number", IsUnique = true)]
    [Index(nameof(ApplicationId), Name = "uccc_application_id", IsUnique = true)]
    public partial class ServiceDeed : IHaveIdProp<long>, IHaveStatusId
    {
        public ServiceDeed()
        {
            Groups = new HashSet<ServiceDeedGroup>();
            Signs = new HashSet<ServiceDeedSign>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(30)]
        public string DocNumber { get; set; }
        [Column("id2")]
        public Guid Id2 { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("contractor_id")]
        public long ContractorId { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("srv_contractor_id")]
        public long SrvContractorId { get; set; }
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
        [Column("web_imzo_secret_key")]
        public string? WebImzoSecretKey { get; set; }
        [Column("web_imzo_request_id")]
        public Guid? WebImzoRequestId { get; set; }
        [Column("message")]
        public string? Message { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(SrvContractorId))]
        public virtual ServiceContract SrvContract { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(ServiceDeedGroup.Owner))]
        public virtual ICollection<ServiceDeedGroup> Groups { get; set; }
        [InverseProperty(nameof(ServiceDeedSign.Owner))]
        public virtual ICollection<ServiceDeedSign> Signs { get; set; }
    }
}
