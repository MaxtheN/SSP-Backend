using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("doc_contractor_category_criterion", Schema = "memship")]
public partial class ContractorCategoryCriterion : IHaveIdProp<long>, IHaveStatusId
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("contractor_category_id")]
    public int ContractorCategoryId { get; set; }
    [Column("min_amount")]
    [Precision(18, 2)]
    public decimal? MinAmount { get; set; }
    [Column("max_amount")]
    [Precision(18, 2)]
    public decimal? MaxAmount { get; set; }
    [Column("expiration_date")]
    public DateOnly ExpirationDate { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(50)]
    public string DocNumber { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ContractorCategoryId))]
    public virtual ContractorCategory ContractorCategory { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
}
