using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Claim
{
    [Table("doc_claim_application", Schema = "claim")]
    [Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
    public partial class ClaimApplication : IHaveIdProp<long>, IBaseApplicationEntity, ILinkToEntity<EmployeeManage>
    {
        public ClaimApplication()
        {
            Files = new HashSet<ClaimApplicationFile>();
            Tables = new HashSet<ClaimApplicationTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("application_id")]
        public long ApplicationId { get; set; }

        [Column("memship_contract_id")]
        public long MemshipContractId { get; set; }

        [Column("memship_certificate_id")]
        public long? MemshipCertificateId { get; set; }

        [Column("prev_application_id")]
        public long? PrevApplicationId { get; set; }

        [Column("claim_application_type_id")]
        public int ClaimApplicationTypeId { get; set; }

        [Column("claim_theme_id")]
        public int ClaimThemeId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("contract_identification_number")]
        [StringLength(100)]
        public string? ContractIdentificationNumber { get; set; }

        [Column("total_amount")]
        [Precision(18, 2)]
        public decimal? TotalAmount { get; set; }

        [Column("main_debt")]
        [Precision(18, 2)]
        public decimal? MainDebt { get; set; }

        [Column("calculed_penalty")]
        [Precision(18, 2)]
        public decimal? CalculedPenalty { get; set; }

        [Column("penalty")]
        [Precision(18, 2)]
        public decimal? Penalty { get; set; }

        [Column("percent")]
        [Precision(18, 2)]
        public decimal? Percent { get; set; }

        [Column("current_principal_interest")]
        [Precision(18, 2)]
        public decimal? CurrentPrincipalInterest { get; set; }

        [Column("current_interest_rate")]
        [Precision(18, 2)]
        public decimal? CurrentInterestRate { get; set; }

        [Column("other_debt_repayment")]
        [Precision(18, 2)]
        public decimal? OtherDebtRepayment { get; set; }

        [Column("currency_id")]
        public int CurrencyId { get; set; }

        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }

        [Column("duration_given_performer", TypeName = "timestamp without time zone")]
        public DateTime? DurationGivenPerformer { get; set; }

        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("bank_branch_name")]
        public string? BankBranchName { get; set; }
        [Column("bank_responsible_person")]
        public string? BankResponsiblePerson { get; set; }
        [Column("message")]
        public string? Message { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty(nameof(EfClasses.Application.ClaimApplication))]
        public virtual Application Application { get; set; }
        [ForeignKey(nameof(PrevApplicationId))]
        public virtual Application? PrevApplication { get; set; }
        [ForeignKey(nameof(ClaimApplicationTypeId))]
        public virtual ClaimApplicationType ClaimApplicationType { get; set; }
        [ForeignKey(nameof(ClaimThemeId))]
        public virtual ClaimTheme ClaimTheme { get; set; }
        [ForeignKey(nameof(CreatedUserId))]
        public virtual BusinessmanUser CreatedUser { get; set; }
        [ForeignKey(nameof(MemshipCertificateId))]
        public virtual MemshipCertificate? MemshipCertificate { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }
        [ForeignKey(nameof(MemshipContractId))]
        public virtual MemshipContract MemshipContract { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [InverseProperty(nameof(ClaimApplicationFile.Owner))]
        public virtual ICollection<ClaimApplicationFile> Files { get; set; }
        [InverseProperty(nameof(ClaimApplicationTable.Owner))]
        public virtual ICollection<ClaimApplicationTable> Tables { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organization { get; set; }
    }
}
