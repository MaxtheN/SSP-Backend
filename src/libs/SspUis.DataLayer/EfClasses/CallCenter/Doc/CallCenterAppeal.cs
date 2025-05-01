using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Appeal;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_call_center_appeal")]
    public partial class CallCenterAppeal : IHaveIdProp<long>, IHaveStatusId
    {
        public CallCenterAppeal()
        {
            Files = new HashSet<CallCenterAppealFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("doc_number")]
        [StringLength(50)]
        public string? DocNumber { get; set; }
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
        [Required]
        [Column("details")]
        [StringLength(2500)]
        public string Details { get; set; }
        [Required]
        [Column("phonenumber")]
        [StringLength(50)]
        public string Phonenumber { get; set; }
        [Column("appeal_type_arrive_id")]
        public int? AppealTypeArriveId { get; set; }
        [Column("appeal_description_id")]
        public int? AppealDescriptionId { get; set; }
        [Column("department_id")]
        public int? DepartmentId { get; set; }
        [Column("country_id")]
        public int? CountryId { get; set; }
        [Column("contractor_category_id")]
        public int? ContractorCategoryId { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string Email { get; set; }
        [Column("oked_id")]
        public int? OkedId { get; set; }
        [Column("address")]
        [StringLength(250)]
        public string Address { get; set; }
        [Column("person_full_name", TypeName = "character varying")]
        public string PersonFullName { get; set; }
        [Column("isimporter")]
        public bool Isimporter { get; set; }
        [Column("isexporter")]
        public bool Isexporter { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("contractor_director_pinfl")]
        [StringLength(50)]
        public string ContractorDirectorPinfl { get; set; }
        [Column("contractor_director_passport_seria")]
        [StringLength(50)]
        public string ContractorDirectorPassportSeria { get; set; }
        [Column("summary")]
        [StringLength(150)]
        public string? Summary { get; set; }
        [Column("contractor_director_passport_number")]
        [StringLength(50)]
        public string ContractorDirectorPassportNumber { get; set; }
        [Column("fcontractor_director_birth_date", TypeName = "timestamp without time zone")]
        public DateTime? FcontractorDirectorBirthDate { get; set; }

        [ForeignKey(nameof(AppealDescriptionId))]
        public virtual AppealDescription AppealDescription { get; set; }
        [ForeignKey(nameof(AppealFormatTypeId))]
        public virtual AppealFormatType AppealFormatType { get; set; }
        [ForeignKey(nameof(ContractorCategoryId))]
        public virtual ContractorCategory ContractorCategory { get; set; }
        [ForeignKey(nameof(AppealTypeArriveId))]
        public virtual AppealTypeArrive AppealTypeArrive { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(AppealTypeId))]
        public virtual AppealType AppealType { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person Person { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual User CreatedUser { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(OkedId))]
        public virtual Oked Oked { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(CallCenterAppealFile.Owner))]
        public virtual ICollection<CallCenterAppealFile> Files { get; set; }
        [InverseProperty(nameof(ExternalDocumentFromEdoc.CallCenterAppeal))]
        public virtual ExternalDocumentFromEdoc EdocIncomingDocInfo { get; set; }
    }
}
