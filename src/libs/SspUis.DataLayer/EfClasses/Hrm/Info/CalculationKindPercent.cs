using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_calculation_kind_percent", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_info_calculation_kind_allowed_doc__owner")]
    public class CalculationKindPercent : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("limit_oper_type")]
        public int? LimitOperTypeId { get; set; }
        [Column("date_on")]
        public DateOnly DateOn { get; set; }
        [Column("percent_rate")]
        [Precision(18, 2)]
        public decimal PercentRate { get; set; }
        [Column("amount")]
        [Precision(18, 2)]
        public decimal? Amount { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(LimitOperTypeId))]
        public virtual LimitOperType LimitOperType { get; set; } 
        [ForeignKey(nameof(OwnerId))]
        public virtual CalculationKind Owner { get; set; } = null!;
    }

}
