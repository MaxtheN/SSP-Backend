using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_mass_planned_calculation", Schema = "hrm")]
    public partial class MassPlannedCalculation : IHaveIdProp<long>, IHaveStatusId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(30)]
        public string DocNumber { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("calculation_kind_id")]
        public int CalculationKindId { get; set; }
        [Column("org_settlement_account_id")]
        public long OrgSettlementAccountId { get; set; }
        [Column("is_cancelation")]
        public bool? IsCancelation { get; set; }
        [Column("percentage")]
        [Precision(18, 2)]
        public decimal? Percentage { get; set; }
        [Column("amount")]
        [Precision(18, 2)]
        public decimal? Amount { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("rounding_type_id")]
        public int RoundingTypeId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(CalculationKindId))]
        public virtual CalculationKind CalculationKind { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(OrgSettlementAccountId))]
        public virtual OrganizationSettlementAccount OrgSettlementAccount { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RoundingTypeId))]
        public virtual RoundingType RoundingType { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
