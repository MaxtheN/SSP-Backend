using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale", Schema = "hrm")]
public partial class TariffScale : IHaveIdProp<int>, IHaveStateId
{
    public TariffScale()
    {
        Tables = new HashSet<TariffScaleTable>();
        Translates = new HashSet<TariffScaleTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("order_code")]
    [StringLength(50)]
    public string OrderCode { get; set; }
    [Required]
    [Column("code")]
    [StringLength(9)]
    public string Code { get; set; }
    [Required]
    [Column("short_name")]
    [StringLength(250)]
    public string ShortName { get; set; }
    [Required]
    [Column("full_name")]
    [StringLength(500)]
    public string FullName { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("tariff_scale_type_id")]
    public int TariffScaleTypeId { get; set; }
    [Column("minimum_value_type_id")]
    public int MinimumValueTypeId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(MinimumValueTypeId))]
    public virtual MinimumValueType MinimumValueType { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [ForeignKey(nameof(TariffScaleTypeId))]
    public virtual TariffScaleType TariffScaleType { get; set; }

    [InverseProperty(nameof(TariffScaleTable.Owner))]
    public virtual ICollection<TariffScaleTable> Tables { get; set; }
    [InverseProperty(nameof(TariffScaleTranslate.Owner))]
    public virtual ICollection<TariffScaleTranslate> Translates { get; set; }
}
