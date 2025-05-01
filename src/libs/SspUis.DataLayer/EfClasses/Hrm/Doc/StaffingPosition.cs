using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_staffing_position", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_doc_staffing_position__owner")]
    public partial class StaffingPosition : IHaveIdProp<long>
    {
        public StaffingPosition()
        {
            CalcKinds = new HashSet<StaffingCalcKind>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("position_id")]
        public int? PositionId { get; set; }
        [Column("qualification_category_id")]
        public int? QualificationCategoryId { get; set; }
        [Column("tariff_scale_type_id")]
        public int TariffScaleTypeId { get; set; }
        [Column("tariff_scale_id")]
        public int? TariffScaleId { get; set; }
        [Column("rank_id")]
        public int? RankId { get; set; }
        [Column("quantity")]
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Column("total_sum")]
        [Precision(18, 2)]
        public decimal? TotalSum { get; set; }
        [Column("rank_code")]
        [StringLength(4)]
        public string RankCode { get; set; }
        [Column("rank_coef")]
        [Precision(18, 4)]
        public decimal? RankCoef { get; set; }
        [Column("corr_coef")]
        [Precision(18, 4)]
        public decimal? CorrCoef { get; set; }
        [Column("fot")]
        [Precision(18, 2)]
        public decimal? Fot { get; set; }
        [Column("for_month")]
        public int ForMonth { get; set; }
        [Column("salary")]
        [Precision(18, 2)]
        public decimal Salary { get; set; }
        [Column("position_period_id")]
        public int PositionPeriodId { get; set; }
        [Column("position_classification_id")]
        public int? PositionClassificationId { get; set; }
        [Column("position_type_id")]
        public int? PositionTypeId { get; set; }
        [Column("position_category_id")]
        public int? PositionCategoryId { get; set; }
        [Column("order_number")]
        public int? OrderNumber { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Staffing Owner { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(PositionCategoryId))]
        public virtual PositionCategory PositionCategory { get; set; }
        [ForeignKey(nameof(PositionClassificationId))]
        public virtual PositionClassification PositionClassification { get; set; }
        [ForeignKey(nameof(PositionPeriodId))]
        public virtual PositionPeriod PositionPeriod { get; set; }
        [ForeignKey(nameof(PositionTypeId))]
        public virtual PositionType PositionType { get; set; }
        [ForeignKey(nameof(QualificationCategoryId))]
        public virtual QualificationCategory QualificationCategory { get; set; }
        [ForeignKey(nameof(RankId))]
        public virtual TariffScaleTable Rank { get; set; }
        [ForeignKey(nameof(TariffScaleId))]
        public virtual TariffScale TariffScale { get; set; }
        [ForeignKey(nameof(TariffScaleTypeId))]
        public virtual TariffScaleType TariffScaleType { get; set; }
        [InverseProperty(nameof(StaffingCalcKind.Owner))]
        public virtual ICollection<StaffingCalcKind> CalcKinds { get; set; }
    }
}
