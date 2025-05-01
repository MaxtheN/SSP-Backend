

using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi;

[Table("doc_kpi_grating_indicator_table", Schema = "kpi")]
[Index(nameof(OwnerId), Name = "ux_doc_kpi_grating_indicator_table__owner")]
public partial class KpiGratingIndicatorTable : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("min_indicator")]
    [Precision(18, 2)]
    public decimal? MinIndicator { get; set; }
    [Column("max_indicator")]
    [Precision(18, 2)]
    public decimal? MaxIndicator { get; set; }
    [Column("score")]
    public int Score { get; set; }
    [Column("unite_of_measure_id")]
    public int UniteOfMeasureId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]
 
    public virtual KpiGratingIndicator Owner { get; set; }
    [ForeignKey(nameof(UniteOfMeasureId))]
  
    public virtual UniteOfMeasure UniteOfMeasure { get; set; }

}
