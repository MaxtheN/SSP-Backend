using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi;
[Table("doc_kpi_grating_indicator", Schema = "kpi")]
[Index(nameof(OwnerId), Name = "ux_doc_kpi_grating_indicator__owner")]
public partial class KpiGratingIndicator :  IHaveIdProp<long>
{
    public KpiGratingIndicator()
    {
        Tables = new HashSet<KpiGratingIndicatorTable>();
    }

    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("indicator_id")]
    public int IndicatorId { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OwnerId))]

    public virtual KpiGrating Owner { get; set; }
    [InverseProperty(nameof(KpiGratingIndicatorTable.Owner))]
    public virtual ICollection<KpiGratingIndicatorTable> Tables { get; set; }
    [ForeignKey(nameof(IndicatorId))]

    public virtual Indicator Indicator { get; set; }
}
