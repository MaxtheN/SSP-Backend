using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi
{
    [Table("doc_kpi_rating_employee_point", Schema = "kpi")]
    [Index(nameof(OwnerId), Name = "ux_doc_kpi_rating_employee_point__owner")]
    public partial class KpiRatingEmployeePoint : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("indicator_id")]
        public int IndicatorId { get; set; }
        [Column("realamount")]
        public int? Realamount { get; set; }
        [Column("realcount")]
        public decimal? Realcount { get; set; }
        [Column("coreamount")]
        public int? Coreamount { get; set; }
        [Column("corecount")]
        public decimal? Corecount { get; set; }
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
        [InverseProperty(nameof(KpiRatingEmployeeTable.Points))]
        public virtual KpiRatingEmployeeTable Owner { get; set; }
    }
}
