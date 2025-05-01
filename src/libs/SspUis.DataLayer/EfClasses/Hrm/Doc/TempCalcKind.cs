using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("doc_temp_calc_kind", Schema = "hrm")]
[Index(nameof(Id2), Name = "uc_id2", IsUnique = true)]
public partial class TempCalcKind : IHaveIdProp<long>, IHaveStatusId
{
    public TempCalcKind()
    {
        Tables = new HashSet<TempCalcKindTable>();
        Signer = new HashSet<TempCalcKindSigner>();
        Files = new HashSet<TempCalcKindFile>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("id2")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id2 { get; set; }
    [Required]
    [Column("doc_number")]
    [StringLength(30)]
    public string DocNumber { get; set; }
    [Column("doc_on")]
    public DateOnly DocOn { get; set; }
    [Column("details")]
    [StringLength(600)]
    public string Details { get; set; }
	[Column("conclusion_for_print")]
	public string? ConclusionForPrint { get; set; }
    [Column("calculation_kind_id")]
    public int CalculationKindId { get; set; }
    [Column("status_id")]
    public int StatusId { get; set; }
    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("temp_calc_kind_type_id")]
    public int TempCalcKindTypeId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }
    [Column("web_imzo_secret_key")]
    public string? WebImzoSecretKey { get; set; }
    [Column("web_imzo_request_id")]
    public Guid? WebImzoRequestId { get; set; }
    [Column("message")]
    public string? Message { get; set; }
    [ForeignKey(nameof(CalculationKindId))]
    public virtual CalculationKind CalculationKind { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [ForeignKey(nameof(StatusId))]
    public virtual Status Status { get; set; }
    [InverseProperty(nameof(TempCalcKindTable.Owner))]
    public virtual ICollection<TempCalcKindTable> Tables { get; set; }
    [ForeignKey(nameof(TempCalcKindTypeId))]
    public virtual TempCalcKindType TempCalcKindType { get; set; }
    [InverseProperty(nameof(TempCalcKindSigner.Owner))]
    public virtual ICollection<TempCalcKindSigner> Signer { get; set; }
    [InverseProperty(nameof(TempCalcKindFile.Owner))]
    public virtual ICollection<TempCalcKindFile> Files { get; set; }
}
