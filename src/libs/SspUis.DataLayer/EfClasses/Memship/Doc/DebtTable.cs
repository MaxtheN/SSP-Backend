using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_debt_table", Schema = "memship")]
public partial class DebtTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("contractor_id")]
    public long ContractorId { get; set; }
    [Column("debt_amount")]
    [Precision(18, 2)]
    public decimal? DebtAmount { get; set; }
    [Column("entitlement_amount")]
    [Precision(18, 2)]
    public decimal? EntitlementAmount { get; set; }
    [Column("application_type_id")]
    public int ApplicationTypeId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ApplicationTypeId))]
    public virtual ApplicationType ApplicationType { get; set; }
    [ForeignKey(nameof(ContractorId))]
    public virtual Contractor Contractor { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(Debt.Tables))]
    public virtual Debt Owner { get; set; }
}
