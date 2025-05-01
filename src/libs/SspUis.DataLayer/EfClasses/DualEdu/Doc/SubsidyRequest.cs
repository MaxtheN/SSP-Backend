using SspUis.DataLayer.EfClasses.DualEdu.Doc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.DualEdu;

[Table("doc_subsidy_request", Schema = "dual_edu")]
public partial class SubsidyRequest : IHaveIdProp<long>, IHaveStatusId
{
    public SubsidyRequest()
    {
        Files = new();
        Tables = new();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("message")]
    public string Message { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("region_id")]
    public int RegionId { get; set; }
    [Column("district_id")]
    public int DistrictId { get; set; }
    [Column("year")]
    public int Year { get; set; }
    [Column("month")]
    public int Month { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("address")]
    [StringLength(500)]
    public string Address { get; set; }
    [Column("email")]
    [StringLength(100)]
    public string Email { get; set; }
    [Column("phone")]
    [StringLength(20)]
    public string Phone { get; set; }
    [Column("contractor_settlement_account_id")]
    public long ContractorSettlementAccountId { get; set; }


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

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(DistrictId))]
    public virtual District District { get; set; }
    [ForeignKey(nameof(RegionId))]
    public virtual Region Region { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [ForeignKey(nameof(ContractorSettlementAccountId))]
    public virtual ContractorSettlementAccount ContractorSettlementAccount { get; set; }
    [InverseProperty(nameof(SubsidyRequestFile.Owner))]
    public virtual List<SubsidyRequestFile> Files { get; set; }
    [InverseProperty(nameof(SubsidyRequestTable.Owner))]
    public virtual List<SubsidyRequestTable> Tables { get; set; }
    [InverseProperty(nameof(SubsidyRequestSign.Owner))]
    public virtual List<SubsidyRequestSign> Signs { get; set; }
}
