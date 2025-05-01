using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_memship_application", Schema = "memship")]
    [Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
    public partial class MemshipApplication : IHaveIdProp<long>, IBaseApplicationEntity
    {
        public MemshipApplication()
        {
            ChamberServices = new HashSet<MemshipApplicationChamberService>();
            Files = new HashSet<MemshipApplicationFile>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("application_id")]
        public long ApplicationId { get; set; }
        [Column("contractor_activity_type_id")]
        public int? ContractorActivityTypeId { get; set; }
        [Column("contractor_category_id")]
        public int ContractorCategoryId { get; set; }
        [Column("oked_id")]
        public int? OkedId { get; set; }
        [Column("employees_count")]
        public int EmployeesCount { get; set; }
        [Column("yearly_earnings")]
        [Precision(18, 2)]
        public decimal? YearlyEarnings { get; set; }
        [Column("yearly_taxes")]
        [Precision(18, 2)]
        public decimal? YearlyTaxes { get; set; }
        [Column("yearly_export")]
        [Precision(18, 4)]
        public decimal? YearlyExport { get; set; }
        [Column("yearly_import")]
        [Precision(18, 4)]
        public decimal? YearlyImport { get; set; }
        [Column("yearly_manufacture")]
        [Precision(18, 4)]
        public decimal? YearlyManufacture { get; set; }
        [Column("message")]
        [StringLength(1024)]
        public string Message { get; set; }
        [Column("contractor_email")]
        [StringLength(250)]
        public string? ContractorEmail { get; set; }
        [Column("contractor_mobile_phone_number")]
        [StringLength(250)]
        public string? ContractorMobilePhoneNumber { get; set; }
        [Column("contractor_work_phone_number")]
        [StringLength(250)]
        public string? ContractorWorkPhoneNumber { get; set; }
        [Column("contractor_additional_phone_number")]
        [StringLength(250)]
        public string? ContractorAdditionalPhoneNumber { get; set; }
        [Column("contractor_faks")]
        [StringLength(250)]
        public string? ContractorFaks { get; set; }
        [Column("contractor_skype")]
        [StringLength(250)]
        public string? ContractorSkype { get; set; }
        [Column("contractor_facebook")]
        [StringLength(250)]
        public string? ContractorFacebook { get; set; }
        [Column("contractor_telegram")]
        [StringLength(250)]
        public string? ContractorTelegram { get; set; }
        [Column("contractor_web_site")]
        [StringLength(250)]
        public string? ContractorWebSite { get; set; }
        [Column("signed_data")]
        [StringLength(10000)]
        public string? SignedData { get; set; }
        [Column("sign_file")]
        [StringLength(250)]
        public Guid? SignFile { get; set; }
        [Column("data_file")]
        public Guid? DataFile { get; set; }
        [Column("signed_user_info")]
        [StringLength(500)]
        public string? SignedUserInfo { get; set; }


        [Column("choosed_location")]
        public bool ChooseLocation { get; set; }
        [Column("choosed_region_id")]
        public int? ChoosedRegionId { get; set; }
        [Column("choosed_district_id")]
        public int? ChoosedDistrictId { get; set; }
        [Column("can_pay_divided")]
        public bool CanPayDivided { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; }
        //[Column("organization_id")]
        //public int? OrganizationId { get; set; }


        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty(nameof(EfClasses.Application.MemshipApplication))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(OkedId))]
        public virtual Oked Oked { get; set; }
        [ForeignKey(nameof(ContractorActivityTypeId))]
        public virtual ContractorActivityType ContractorActivityType { get; set; }
        [ForeignKey(nameof(ContractorCategoryId))]
        public virtual ContractorCategory ContractorCategory { get; set; }
        //ochirib turdik vohtincha erpdan arizani create qilishda hato bergani uchun
        //agar muhum narsa bosa ochish kere boladi
        //[ForeignKey(nameof(CreatedUserId))]
        //public virtual BusinessmanUser CreatedUser { get; set; }


        [ForeignKey(nameof(ChoosedRegionId))]
        public virtual Region ChoosedRegion { get; set; }
        [ForeignKey(nameof(ChoosedDistrictId))]
        public virtual District ChoosedDistrict { get; set; }


        [InverseProperty(nameof(MemshipApplicationChamberService.Owner))]
        public virtual ICollection<MemshipApplicationChamberService> ChamberServices { get; set; }
        [InverseProperty(nameof(MemshipApplicationFile.Owner))]
        public virtual ICollection<MemshipApplicationFile> Files { get; set; }
    }
}
