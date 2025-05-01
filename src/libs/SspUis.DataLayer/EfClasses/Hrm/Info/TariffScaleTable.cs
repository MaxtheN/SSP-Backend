using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale_table", Schema = "hrm")]
[Index(nameof(OwnerId), Name = "ix_info_tariff_scale_table__owner")]
public partial class TariffScaleTable : IHaveIdProp<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("owner_id")]
    public int OwnerId { get; set; }
    [Required]
    [Column("rank_code")]
    [StringLength(5)]
    public string RankCode { get; set; }
    [Column("order_code")]
    public int? OrderCode { get; set; }

    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(TariffScale.Tables))]
    public virtual TariffScale Owner { get; set; }
}
