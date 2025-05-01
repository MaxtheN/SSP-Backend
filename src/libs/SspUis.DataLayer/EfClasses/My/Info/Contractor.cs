using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("info_contractor", Schema = "my")]
    public partial class Contractor : IHaveStateId, IHaveIdProp<long>
    {
        public Contractor()
        {
            BusinessmanUserInContractors = new HashSet<BusinessmanUserInContractor>();
            Okeds = new HashSet<ContractorOked>();
            SettlementAccounts = new HashSet<ContractorSettlementAccount>();
            Contacts = new HashSet<ContractorContact>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(250)]
        public string FullName { get; set; }
        [Column("inn")]
        [StringLength(9)]
        public string Inn { get; set; }
        [Column("pinfl")]
        [StringLength(14)]
        public string Pinfl { get; set; }
        [Column("oked_id")]
        public int? OkedId { get; set; }
        [Column("bank_id")]
        public int? BankId { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; }
        [Column("accounter")]
        [StringLength(250)]
        public string Accounter { get; set; }
        [Column("director")]
        [StringLength(250)]
        public string Director { get; set; }
        [Column("phone_number")]
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [Column("contact_info")]
        [StringLength(250)]
        public string Contact { get; set; }
        [Column("vat_code")]
        [StringLength(30)]
        public string VatCode { get; set; }

        [Column("organization_legal_form_id")]
        public int? OrganizationLegalFormId { get; set; }

        [Column("registration_date")]
        public DateOnly RegistrationDate { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("gov_share")]
        public decimal? GovShare { get; set; }
        [Column("opf_id")]
        public int? OpfId { get; set; }
        [Column("kfs")]
        public int? Kfs { get; set; }
        [Column("soogu")]
        public string Soogu { get; set; } = null;
        [Column("soogu_registrator")]
        public string SooguRegistrator { get; set; } = null;
        [Column("registration_number")]
        public string RegistrationNumber { get; set; } = null;
        [Column("business_fund")]
        public decimal? BusinessFund { get; set; }
        [Column("village_code")]
        public int? VillageCode { get; set; }
        [Column("village_name")]
        public string VillageName { get; set; }
        [Column("owner_name")]
        public string? OwnerName { get; set; }
        [Column("tax_rate")]
        public decimal? TaxRate { get; set; }
        [Column("avg_number_employees")]
        public int? AvgNumberEmployees { get; set; }
        [Column("monthly_number_employees")]
        public int? MonthlyNumberEmployees { get; set; }
        [Column("is_last_offer")]
        public bool IsLastOffer { get; set; }
        [ForeignKey(nameof(BankId))]
        [InverseProperty(nameof(EfClasses.Bank.Contractors))]
        public virtual Bank Bank { get; set; }
        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(EfClasses.Country.Contractors))]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [InverseProperty(nameof(EfClasses.District.Contractors))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(OkedId))]
        [InverseProperty(nameof(EfClasses.Oked.Contractors))]
        public virtual Oked Oked { get; set; }
        [ForeignKey(nameof(RegionId))]
        [InverseProperty(nameof(EfClasses.Region.Contractors))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }
        [ForeignKey(nameof(OpfId))]
        public virtual Opf Opf { get; set; }
        [ForeignKey(nameof(OrganizationLegalFormId))]
        public virtual OrganizationLegalForm OrganizationLegalForm { get; set; }
        [InverseProperty(nameof(BusinessmanUserInContractor.Contractor))]
        public virtual ICollection<BusinessmanUserInContractor> BusinessmanUserInContractors { get; set; }
        [InverseProperty(nameof(ContractorOked.Owner))]
        public virtual ICollection<ContractorOked> Okeds { get; set; }
        [InverseProperty(nameof(ContractorSettlementAccount.Owner))]
        public virtual ICollection<ContractorSettlementAccount> SettlementAccounts { get; set; }
        [InverseProperty(nameof(ContractorContact.Owner))]
        public virtual ICollection<ContractorContact> Contacts { get; set; }
        [InverseProperty(nameof(BusinessActivityType.Contractor))]
        public virtual ICollection<BusinessActivityType> BusinessActivityTypes { get; set; }

        [InverseProperty(nameof(Application.Contractor))]
        public virtual ICollection<Application> Applications { get; set; }

        [InverseProperty(nameof(MemshipContract.Contractor))]
        public virtual ICollection<MemshipContract> MemshipContracts { get; set; }

        [InverseProperty(nameof(MemshipCertificate.Contractor))]
        public virtual ICollection<MemshipCertificate> MemshipCertificates { get; set; }
    }
}
