using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_fixed_minimum_value", Schema = "hrm")]
    public class FixedMinimumValue : IHaveIdProp<long>, IHaveStateId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("minimum_value_type_id")]
        public int MinimumValueTypeId { get; set; }
        [Column("date_on")]
        public DateOnly DateOn { get; set; }
        [Column("fixed_value")]
        [Precision(18, 2)]
        public decimal FixedValue { get; set; }
        [Column("change_percentage")]
        [Precision(18, 2)]
        public decimal ChangePercentage { get; set; }
        [Required]
        [Column("normative_doc")]
        [StringLength(350)]
        public string NormativeDoc { get; set; } = null!;
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

        [ForeignKey(nameof(MinimumValueTypeId))]
        public virtual MinimumValueType MinimumValueType { get; set; } = null!;
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; } = null!;
    }
}
