using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi;


[Table("doc_kpi_plan_employee_indicator_creates", Schema = "kpi")]
[Index(nameof(OwnerId), Name = "ux_doc_kpi_plan_employee_indicator_creates_owner")]
public partial class KpiPlanEmployeeIndicatorCreate : IHaveIdProp<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }
    [Column("owner_id")]
    public long OwnerId { get; set; }
    [Column("indicator_id")]
    public int IndicatorId { get; set; }
    [Column("amount")]
    public int? Amount { get; set; }
    [Column("count")]
    public decimal? Count { get; set; }
    [Column("created_at", TypeName = "timestamp without time zone")]

    
    public DateTime CreatedAt { get; set; }
    
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }



    [ForeignKey(nameof(IndicatorId))]
    public virtual Indicator Indicator { get; set; }
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(KpiPlanForEmployeeTable.Creates))]
    public virtual KpiPlanForEmployeeTable Owner { get; set; }
}
