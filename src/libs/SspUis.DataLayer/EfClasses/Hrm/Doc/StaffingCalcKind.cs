using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_staffing_calc_kind", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_doc_staffing_calc_kind__owner")]
    public partial class StaffingCalcKind : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("calculation_kind_id")]
        public int? CalculationKindId { get; set; }
        [Column("calc_coef")]
        [Precision(18, 4)]
        public decimal? CalcCoef { get; set; }
        [Column("calc_sum")]
        [Precision(18, 2)]
        public decimal? CalcSum { get; set; }

        [ForeignKey(nameof(CalculationKindId))]
        public virtual CalculationKind CalculationKind { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(StaffingPosition.CalcKinds))]
        public virtual StaffingPosition Owner { get; set; }
    }
}
