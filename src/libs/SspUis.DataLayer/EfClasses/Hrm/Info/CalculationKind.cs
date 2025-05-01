using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("info_calculation_kind", Schema = "hrm")]
    public partial class CalculationKind : IHaveIdProp<int>, IHaveStateId
    {
        public CalculationKind()
        {
            Translates = new HashSet<CalculationKindTranslate>();
            Percents = new HashSet<CalculationKindPercent>();
            UsedTables = new HashSet<CalculationKindUsedTable>();
            AllowedDocs = new HashSet<CalculationKindAllowedDoc>();
            CalculationStructure = new HashSet<CalculationKindStructure>();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("order_code")]
        [StringLength(50)]
        public string OrderCode { get; set; }
        [Required]
        [Column("code")]
        [StringLength(9)]
        public string Code { get; set; }
        [Required]
        [Column("short_name")]
        [StringLength(250)]
        public string ShortName { get; set; }
        [Required]
        [Column("full_name")]
        [StringLength(500)]
        public string FullName { get; set; }
        [Required]
        [Column("normative_doc")]
        [StringLength(600)]
        public string NormativeDoc { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("item_of_expense_id")]
        public int? ItemOfExpenseId { get; set; }
        [Column("calculation_type_id")]
        public int CalculationTypeId { get; set; }
        [Column("calculation_method_id")]
        public int? CalculationMethodId { get; set; }
        [Column("minimum_value_type_id")]
        public int? MinimumValueTypeId { get; set; }
        [Column("calculate_by_time_type_id")]
        public int? CalculateByTimeTypeId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("depend_on_rate")]
        public bool DependOnRate { get; set; }
        [Column("by_enrolment")]
        public bool ByEnrolment { get; set; }
        [Column("is_mandatory")]
        public bool IsMandatory { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(CalculateByTimeTypeId))]
        public virtual CalculateByTimeType CalculateByTimeType { get; set; }
        [ForeignKey(nameof(CalculationMethodId))]
        public virtual CalculationMethod CalculationMethod { get; set; }
        [ForeignKey(nameof(CalculationTypeId))]
        public virtual CalculationType CalculationType { get; set; }
        [ForeignKey(nameof(ItemOfExpenseId))]
        public virtual ItemOfExpense ItemOfExpense { get; set; }
        [ForeignKey(nameof(MinimumValueTypeId))]
        public virtual MinimumValueType MinimumValueType { get; set; }
        [ForeignKey(nameof(StateId))]
        public virtual State State { get; set; }

        [InverseProperty(nameof(CalculationKindTranslate.Owner))]
        public virtual ICollection<CalculationKindTranslate> Translates { get; set; }
        [InverseProperty(nameof(CalculationKindPercent.Owner))]
        public virtual ICollection<CalculationKindPercent> Percents { get; set; }
        [InverseProperty(nameof(CalculationKindUsedTable.Owner))]
        public virtual ICollection<CalculationKindUsedTable> UsedTables { get; set; }
        [InverseProperty(nameof(CalculationKindUsedTable.Owner))]
        public virtual ICollection<CalculationKindAllowedDoc> AllowedDocs { get; set; }
        [InverseProperty(nameof(CalculationKindStructure.Owner))]
        public virtual ICollection<CalculationKindStructure> CalculationStructure { get; set; }
    }
}
