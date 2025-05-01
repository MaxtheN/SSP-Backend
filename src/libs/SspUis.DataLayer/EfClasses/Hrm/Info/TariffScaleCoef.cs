using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("info_tariff_scale_coef", Schema = "hrm")]
public partial class TariffScaleCoef : IHaveIdProp<int>, IHaveStateId
{
    public TariffScaleCoef()
    {
        Tables = new HashSet<TariffScaleCoefTable>();
        Translates = new HashSet<TariffScaleCoefTranslate>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("date_on")]
    public DateOnly DateOn { get; set; }
    [Column("tariff_scale_id")]
    public int TariffScaleId { get; set; }
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

    [ForeignKey(nameof(TariffScaleId))]
    public virtual TariffScale TariffScale { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }
    [InverseProperty(nameof(TariffScaleCoefTable.Owner))]
    public virtual ICollection<TariffScaleCoefTable> Tables { get; set; }
    [InverseProperty(nameof(TariffScaleCoefTranslate.Owner))]
    public virtual ICollection<TariffScaleCoefTranslate> Translates { get; set; }
}
