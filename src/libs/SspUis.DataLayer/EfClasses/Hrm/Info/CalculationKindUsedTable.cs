using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_calculation_kind_used_table", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_info_calculation_kind_used_table__owner")]
    public class CalculationKindUsedTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("calc_from_in_sum")]
        public bool CalcFromInSum { get; set; }
        [Column("formed_calculation_id")]
        public int FormedCalculationKindId { get; set; }
        [Column("minimum_value_type_id")]
        public int? MinimumValueTypeId { get; set; }
        [Column("quantity_of_minimum_value")]
        [Precision(18, 2)]
        public decimal? QuantityOfMinimumValue { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }


        [ForeignKey(nameof(MinimumValueTypeId))]
        public virtual MinimumValueType MinimumValueType { get; set; } = null!;

        [ForeignKey(nameof(FormedCalculationKindId))]
        public virtual CalculationKind FormedCalculationKind { get; set; } = null!;
        [ForeignKey(nameof(OwnerId))]
        public virtual CalculationKind Owner { get; set; } = null!;
    }
}
