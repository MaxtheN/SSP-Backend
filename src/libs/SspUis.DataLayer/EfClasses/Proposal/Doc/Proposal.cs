using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.EfCode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Proposal
{
    [Table("doc_proposal", Schema = "propos")]
    public class Proposal : IHaveIdProp<long>, IDocument
    {
        public Proposal()
        {
            ProposalFiles = new HashSet<ProposalFile>();
        }
    
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("company_name", TypeName = "character varying")]
        public string CompanyName { get; set; }
        [Column("company_inn")]
        [StringLength(9)]
        public string CompanyInn { get; set; }
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("doc_on")]
        public DateOnly? DocOn { get; set; }
        [Column("proposal_type_id")]
        public int? ProposalTypeId { get; set; }
        [Column("business_sector_id")]
        public int? BusinessSectorId { get; set; }
        [Column("external_source_type_id")]
        public int ExternalSourceTypeId { get; set; }
        [Column("surname_latin")]
        [StringLength(100)]
        public string SurnameLatin { get; set; }
        [Column("name_latin")]
        [StringLength(100)]
        public string NameLatin { get; set; }
        [Column("patronym_latin")]
        [StringLength(100)]
        public string PatronymLatin { get; set; }
        [Column("surname_eng")]
        [StringLength(100)]
        public string SurnameEng { get; set; }
        [Column("name_eng")]
        [StringLength(100)]
        public string NameEng { get; set; }
        [Column("birth_date", TypeName = "timestamp without time zone")]
        public DateTime? BirthDate { get; set; }
        [Column("gender_id")]
        public int? GenderId { get; set; }
        [Column("phone_number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string Email { get; set; }
        [Column("region_id")]
        public int? RegionId { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("mfy_id")]
        public long? MfyId { get; set; }
        [Column("address")]
        [StringLength(200)]
        public string Address { get; set; }
        [Column("employement_type_id")]
        public int? EmployementTypeId { get; set; }
        [Column("proposal_subject_id")]
        public int? ProposalSubjectId { get; set; }
        [Column("to_organization_id")]
        public int? ToOrganizationId { get; set; }
        [Column("proposal_disclosure_id")]
        public int? ProposalDisclosureId { get; set; }
        [Column("proposal_text")]
        public string ProposalText { get; set; }
        [Column("company_type_id")]
        public int? CompanyTypeId { get; set; }
        [Column("appeal_text")]
        public string AppealText { get; set; }
        [Column("status_id")]
        public int? StatusId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime? CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
    
        [ForeignKey(nameof(BusinessSectorId))]
        public virtual BusinessSector BusinessSector { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(ExternalSourceTypeId))]
        public virtual ExternalSourceType ExternalSourceType { get; set; }

        [ForeignKey(nameof(EmployementTypeId))]
        public virtual EmploymentType EmployementType { get; set; }
        [ForeignKey(nameof(ProposalTypeId))]
        public virtual ApplicantType ProposalType { get; set; }
        [ForeignKey(nameof(GenderId))]
        public virtual Gender Gender { get; set; }
        [ForeignKey(nameof(MfyId))]
        public virtual Mfy Mfy { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(ProposalDisclosureId))]
        public virtual ProposalDisclosure ProposalDisclosure { get; set; }
        [ForeignKey(nameof(ProposalSubjectId))]
        public virtual ProposalSubject ProposalSubject { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(ToOrganizationId))]
        public virtual Organization ToOrganization { get; set; }
        [ForeignKey(nameof(CompanyTypeId))]
        public virtual CompanyType CompanyType { get; set; }
        [InverseProperty(nameof(ProposalFile.Owner))]
        public virtual ICollection<ProposalFile> ProposalFiles { get; set; }
    }
}
