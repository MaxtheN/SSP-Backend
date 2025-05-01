using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_employee_missed_day", Schema = "hrm")]
    public partial class EmployeeMissedDay : IHaveIdProp<long>, IHaveStatusId
    {
        public EmployeeMissedDay()
        {
            Tables = new HashSet<EmployeeMissedDayTable>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Required]
        [Column("doc_number")]
        [StringLength(30)]
        public string DocNumber { get; set; }
        [Column("doc_date")]
        public DateOnly DocDate { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("for_all_employee")]
        public bool ForAllEmployee { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("department_id")]
        public int? DepartmentId { get; set; }
        [Column("position_id")]
        public int? PositionId { get; set; }
        [Column("start_on")]
        public DateOnly? StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("date_of_created", TypeName = "timestamp without time zone")]
        public DateTime DateOfCreated { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("date_of_modified", TypeName = "timestamp without time zone")]
        public DateTime? DateOfModified { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
        [InverseProperty(nameof(EmployeeMissedDayTable.Owner))]
        public virtual ICollection<EmployeeMissedDayTable> Tables { get; set; }
    }
}
