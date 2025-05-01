using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Appeal
{
    [Table("doc_appeal_application", Schema = "appeal")]
    public partial class AppealApplication : IHaveIdProp<long>, IHaveStatusId
    {
        public AppealApplication()
        {
            Files = new HashSet<AppealApplicationFile>();
            Signs = new HashSet<AppealApplicationSign>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("person_full_name")]
        public string PersonFullName { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("details")]
        [StringLength(2500)]
        public string? Details { get; set; }
        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("person_id")]
        public int? PersonId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("busyness")]
        public bool Busyness { get; set; }
        [Column("appeal_type_id")]
        public int AppealTypeId { get; set; }
        [Column("appeal_format_type_id")]
        public int AppealFormatTypeId { get; set; }
        [Column("open_appeal")]
        public bool OpenAppeal { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("appeal_type_arrive_id")]
        public int? AppealTypeArriveId { get; set; }
        [Column("appeal_description_id")]
        public int? AppealDescriptionId { get; set; }
        [Column("department_id")]
        public int? DepartmentId { get; set; }
        [Column("message")]
        public string? Message { get; set; }
        [Column("is_created_by_chamber")]
        public bool IsCreatedByChamber { get; set; }
        [Column("phonenumber")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string? Email { get; set; }
        [Column("address")]
        [StringLength(250)]
        public string? Address { get; set; }
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

        [ForeignKey(nameof(AppealFormatTypeId))]
        public virtual AppealFormatType AppealFormatType { get; set; }
        [ForeignKey(nameof(AppealTypeId))]
        public virtual AppealType AppealType { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor? Contractor { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual BusinessmanUser CreatedUser { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(AppealTypeArriveId))]
        public virtual AppealTypeArrive AppealTypeArrive { get; set; }
        [ForeignKey(nameof(AppealDescriptionId))]
        public virtual AppealDescription AppealDescription { get; set; }
        [InverseProperty(nameof(AppealApplicationFile.Owner))]
        public virtual ICollection<AppealApplicationFile> Files { get; set; }
        [InverseProperty(nameof(AppealApplicationSign.Owner))]
        public virtual ICollection<AppealApplicationSign> Signs { get; set; }
        [InverseProperty(nameof(ExternalDocumentFromEdoc.AppealApplication))]
        public virtual ExternalDocumentFromEdoc EdocInfoForList { get; set; }
    }
}
