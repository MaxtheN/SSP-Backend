using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_employee_sick_leave", Schema = "hrm")]
    public class EmployeeSickLeave : IHaveIdProp<long>, IHaveStatusId
    {
        public EmployeeSickLeave()
        {
            Tables = new HashSet<EmployeeSickLeaveTable>();
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
        [Column("conclusion_for_print")]
        public string ConclusionForPrint { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("employee_sick_leave_type_id")]
        public int EmployeeSickLeaveTypeId { get; set; }
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
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(EmployeeSickLeaveTypeId))]
        public virtual EmployeeSickLeaveType EmployeeSickLeaveType { get; set; }
        [InverseProperty(nameof(EmployeeSickLeaveTable.Owner))]
        public virtual ICollection<EmployeeSickLeaveTable> Tables { get; set; }
    }
}
