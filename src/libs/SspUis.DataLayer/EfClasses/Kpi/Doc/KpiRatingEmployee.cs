using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Kpi;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Kpi
{
    [Table("doc_kpi_rating_employee", Schema = "kpi")]
    public partial class KpiRatingEmployee : IHaveIdProp<long>, IHaveStatusId
    {
        public KpiRatingEmployee()
        {
            Tables = new HashSet<KpiRatingEmployeeTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(50)]
        public string DocNumber { get; set; }
        [Column("plan_for_employee_id")]
        public long PlanForEmployeeId { get; set; }
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

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PlanForEmployeeId))]
        public virtual KpiPlanForEmployee PlanForEmployee { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(KpiRatingEmployeeTable.Owner))]
        public virtual ICollection<KpiRatingEmployeeTable> Tables { get; set; }
    }
}
