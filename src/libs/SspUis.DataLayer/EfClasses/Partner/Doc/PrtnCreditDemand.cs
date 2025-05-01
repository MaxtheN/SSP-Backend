
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;
namespace SspUis.DataLayer.EfClasses;

[Table("doc_prtn_credit_demand", Schema = "partner")]
public class PrtnCreditDemand :PrtnCreditDemandDlDto<UpdatePrtnCreditDemandDlDto>, IHaveIdProp<long>, IHaveStatusId
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("certificate_id")]
    public long CertificateId { get; set; }
    [Column("application_id")]
    public long ApplicationId { get; set; }
    [Column("implemented_project_name")]
    public string ImplementedProjectName { get; set; }
    [Column("project_cost")]
    public double ProjectCost { get; set; }
    [Column("own_investment")]
    public double OwnInvestment { get; set; }
    [Column("foreign_investment")]
    public double ForeignInvestment { get; set; }
    [Column("privilege_bank_credit")]
    public double PrivillageBankCredit { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("bank_id")]
    public int BankId { get; set; }
    [Column("region_id")]
    public int RegionId { get; set; }
    [Column("district_id")]
    public int DistrictId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }

    [ForeignKey(nameof(CertificateId))]
    public virtual PrtnCertificate PrtnCertificate { get; set; }

    [ForeignKey(nameof(ApplicationId))]
    public virtual PrtnApplication PrtnApplication { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }

    [ForeignKey(nameof(RegionId))]
    public virtual Region Region { get; set; }

    [ForeignKey(nameof(DistrictId))]
    public virtual District District { get; set; }

    [ForeignKey(nameof(BankId))]
    public virtual Bank Bank { get; set; }

    [ForeignKey(nameof(CreatedUserId))]
    public virtual BusinessmanUser BusinessmanUser { get; set; }

}
