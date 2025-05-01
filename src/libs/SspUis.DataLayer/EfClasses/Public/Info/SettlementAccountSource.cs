using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_settlement_account_source")]
[Index(nameof(Code1), nameof(Code2), nameof(Code3), nameof(Code4), Name = "uc_info_settlement_account_source_another_code", IsUnique = true)]
[Index(nameof(Code), Name = "uc_info_settlement_account_source_code", IsUnique = true)]
public partial class SettlementAccountSource : IHaveIdProp<int>, IHaveStateId
{
    public SettlementAccountSource()
    {
        Children = new HashSet<SettlementAccountSource>();
        Translates = new HashSet<SettlementAccountSourceTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("code")]
    [StringLength(6)]
    public string Code { get; set; }
    [Required]
    [Column("code1")]
    [StringLength(1)]
    public string Code1 { get; set; }
    [Required]
    [Column("code2")]
    [StringLength(3)]
    public string Code2 { get; set; }
    [Required]
    [Column("code3")]
    [StringLength(1)]
    public string Code3 { get; set; }
    [Required]
    [Column("code4")]
    [StringLength(1)]
    public string Code4 { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("parent_id")]
    public int? ParentId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(ParentId))]
    [InverseProperty(nameof(SettlementAccountSource.Children))]
    public virtual SettlementAccountSource? Parent { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(SettlementAccountSource.Parent))]
    public virtual ICollection<SettlementAccountSource> Children { get; set; }
    [InverseProperty(nameof(SettlementAccountSourceTranslate.Owner))]
    public virtual ICollection<SettlementAccountSourceTranslate> Translates { get; set; }
}
