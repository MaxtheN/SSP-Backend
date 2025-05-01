using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_planned_calculation", Schema = "hrm")]
    public partial class PlannedCalculation : IHaveIdProp<long>, IHaveStatusId
    {
        public PlannedCalculation()
        {
            Tables = new HashSet<PlannedCalculationTable>();
        }

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
        public int? DepartmentId { get; set; }
        [Column("calculation_kind_id")]
        public int CalculationKindId { get; set; }
        [Column("is_cancelation")]
        public bool IsCancelation { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
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
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(PlannedCalculationTable.Owner))]
        public virtual ICollection<PlannedCalculationTable> Tables { get; set; }
    }
}
