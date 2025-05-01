using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

using SspUis.DataLayer.EfClasses.Hrm;

using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_organization",Schema ="public")]
    [Index(nameof(Inn), Name = "info_organization_unique_index_inn", IsUnique = true)]
    public partial class Organization : IHaveStateId, IHaveIdProp<int>
    {
        public Organization()
        {
            Translates = new HashSet<OrganizationTranslate>();
            Signs = new HashSet<OrganizationSign>();
            Users = new HashSet<User>();
            Children = new HashSet<Organization>();
            Employees = new HashSet<Employee>();
            SettlementAccounts = new HashSet<OrganizationSettlementAccount>();
            Files = new HashSet<OrganizationFile>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(300)]
        public string FullName { get; set; }
        [Column("inn")]
        [StringLength(9)]
        public string Inn { get; set; }
        [Column("email")]
        [StringLength(250)]
        public string Email { get; set; }
        [Column("parent_id")]
        public int? ParentId { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int? DistrictId { get; set; }
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; }
        [Column("oked_id")]
        public int? OkedId { get; set; }
        [Column("director")]
        [StringLength(250)]
        public string Director { get; set; }
        [Column("accounter")]
        [StringLength(250)]
        public string Accounter { get; set; }
        [Column("vat_code")]
        [StringLength(12)]
        public string VatCode { get; set; }
        [Column("zip_code")]
        [StringLength(50)]
        public string ZipCode { get; set; }
        [Column("settings_json", TypeName = "character varying")]
        public string SettingsJson { get; set; }
        [Column("phone_number")]
        [StringLength(250)]
        public string PhoneNumber { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("sign_organization_type_id")]
        public int? SignOrganizationTypeId { get; set; }
        [Column("organization_legal_form_id")]
        public int? OrganizationLegalFormId { get; set; }
        [Column("organization_group_id")]
        public int OrganizationGroupId { get; set; }
        [Column("incoming_doc_receiver_employee_id")]
        public int? IncomingDocReceiverEmployeeId { get; set; }
        [Column("organizational_structure_id")]
        public int? OrganizationalStructureId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(EfClasses.Country.Organizations))]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(SignOrganizationTypeId))]
        public virtual SignOrganizationType SignOrganizationType { get; set; }
        [ForeignKey(nameof(OrganizationalStructureId))]
        public virtual OrganizationalStructure? OrganizationalStructure { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [InverseProperty(nameof(EfClasses.District.Organizations))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(OkedId))]
        [InverseProperty(nameof(EfClasses.Oked.Organizations))]
        public virtual Oked Oked { get; set; }
        [ForeignKey(nameof(RegionId))]
        [InverseProperty(nameof(EfClasses.Region.Organizations))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(IncomingDocReceiverEmployeeId))]
        public virtual Employee IncomingDocReceiverEmployee { get; set; }
        [ForeignKey(nameof(OrganizationLegalFormId))]
        public virtual OrganizationLegalForm OrganizationLegalForm { get; set; }
        [ForeignKey(nameof(OrganizationGroupId))]
        public virtual OrganizationGroup OrganizationGroup { get; set; }
        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Children))]
        public virtual Organization Parent { get; set; }
        [InverseProperty(nameof(OrganizationTranslate.Owner))]
        public virtual ICollection<OrganizationTranslate> Translates { get; set; }
        [InverseProperty(nameof(OrganizationSign.Owner))]
        public virtual ICollection<OrganizationSign> Signs { get; set; }
        [InverseProperty(nameof(User.Organization))]
        public virtual ICollection<User> Users { get; set; }
        [InverseProperty(nameof(Parent))]
        public virtual ICollection<Organization> Children { get; set; }
        [InverseProperty(nameof(Employee.Organization))]
        public virtual ICollection<Employee> Employees { get; set; }
        [InverseProperty(nameof(OrganizationFile.Owner))]
        public virtual ICollection<OrganizationFile> Files { get; set; }
        [InverseProperty(nameof(OrganizationSettlementAccount.Organization))]
        public virtual ICollection<OrganizationSettlementAccount> SettlementAccounts { get; set; }
    }
}
