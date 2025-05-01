using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses;

[Table("info_organizational_structure_position")]
//[Index(nameof(OwnerId), Name = "ix_info_organizational_structure_position__owner")]
public partial class OrganizationalStructurePosition : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Column("position_id")]
    public int PositionId { get; set; }
    [Column("position_type_id")]
    public int PositionTypeId { get; set; }
    [Column("position_category_id")]
    public int PositionCategoryId { get; set; }
    [Column("tariff_scale_type_id")]
    public int TariffScaleTypeId { get; set; }
    [Column("tariff_scale_id")]
    public int? TariffScaleId { get; set; }
    [Column("rank_id")]
    public int? RankId { get; set; }
    [Column("amount")]
    [Precision(18, 2)]
    public decimal? Amount { get; set; }
    [Column("staffing_quantity")]
    [Precision(18, 4)]
    public decimal? StaffingQuantity { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
    public virtual OrganizationalStructure Owner { get; set; }
    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }
    [ForeignKey(nameof(PositionCategoryId))]
    public virtual PositionCategory PositionCategory { get; set; }
    [ForeignKey(nameof(PositionTypeId))]
    public virtual PositionType PositionType { get; set; }
    [ForeignKey(nameof(RankId))]
    public virtual TariffScaleTable Rank { get; set; }
    [ForeignKey(nameof(TariffScaleId))]
    public virtual TariffScale TariffScale { get; set; }
    [ForeignKey(nameof(TariffScaleTypeId))]
    public virtual TariffScaleType TariffScaleType { get; set; }
}
