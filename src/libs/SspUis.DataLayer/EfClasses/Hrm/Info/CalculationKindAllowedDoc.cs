using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_calculation_kind_allowed_doc", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_info_calculation_kind_allowed_doc__owner")]
    public class CalculationKindAllowedDoc : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("date_on")]
        public DateOnly DateOn { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        [Precision(6)]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        [Precision(10)]
        public int? CreatedUserId { get; set; }
        [Column("modified_at")]
        [Precision(6)]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        [Precision(10)]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual CalculationKind Owner { get; set; } = null!;
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; } = null!;
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; } = null!;
    }
}
