using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale_coef_table", Schema = "hrm")]
public partial class TariffScaleCoefTable : IHaveIdProp<int>
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
    [Column("coef")]
    [Precision(18, 3)]
    public decimal Coef { get; set; }
    [Column("tariff_scale_table_id")]
    public int TariffScaleTableId { get; set; }
    [Column("order_code")]
    public int? OrderCode { get; set; }
    [ForeignKey(nameof(OwnerId))]
    public virtual TariffScaleCoef Owner { get; set; }
    [ForeignKey(nameof(TariffScaleTableId))]
    public virtual TariffScaleTable TariffScaleTable { get; set; }
}
