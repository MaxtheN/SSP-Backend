using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_staffing_indicator_value", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_doc_staffing_indicator_value__owner")]
    public partial class StaffingIndicatorValue : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("staffing_indicator_id")]
        public int StaffingIndicatorId { get; set; }
        [Column("quantity")]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Column("total_sum")]
        [Precision(18, 2)]
        public decimal? TotalSum { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(Staffing.IndicatorValues))]
        public virtual Staffing Owner { get; set; }
        [ForeignKey(nameof(StaffingIndicatorId))]
        public virtual StaffingIndicator StaffingIndicator { get; set; }
    }
}
