using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_execution_application", Schema = "partner")]
    public partial class ExecutionApplication : IHaveIdProp<long>, IHaveStatusId
    {
        public ExecutionApplication()
        {
            Tables = new HashSet<ExecutionApplicationTable>();
            Signs = new HashSet<ExecutionApplicationSign>();
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
        [Column("region_id")]
        public int RegionId { get; set; }
        [Column("district_id")]
        public int DistrictId { get; set; }
        [Column("year")]
        public int Year { get; set; }
        [Column("month")]
        public int Month { get; set; }
        [Column("total_new_vacancies_count")]
        public int TotalNewVacanciesCount { get; set; }
        [Column("total_payment_amount")]
        [Precision(18, 2)]
        public decimal TotalPaymentAmount { get; set; }
        [Column("total_average_salary")]
        [Precision(18, 2)]
        public decimal TotalAverageSalary { get; set; }
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

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [InverseProperty(nameof(ExecutionApplicationTable.Owner))]
        public virtual ICollection<ExecutionApplicationTable> Tables { get; set; }
        [InverseProperty(nameof(ExecutionApplicationSign.Owner))]
        public virtual ICollection<ExecutionApplicationSign> Signs { get; set; }
    }
}