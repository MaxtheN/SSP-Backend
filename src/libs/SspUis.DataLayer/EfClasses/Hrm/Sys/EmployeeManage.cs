using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("sys_employee_manage", Schema = "hrm")]
    public partial class EmployeeManage : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("doc_table_id")]
        public int DocTableId { get; set; }
        [Column("doc_id")]
        public long DocId { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("emp_appoint_order_type_id")]
        public int EmpAppointOrderTypeId { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("position_id")]
        public int PositionId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employment_type_id")]
        public int EmploymentTypeId { get; set; }
        [Column("employment_rate")]
        public decimal? EmploymentRate { get; set; }
        [Column("work_schedule_id")]
        public int WorkScheduleId { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Column("end_by_document_on")]
        public DateOnly? EndByDocumentOn { get; set; }
        [Column("end_table_id")]
        public int? EndTableId { get; set; }
        [Column("end_doc_id")]
        public long? EndDocId { get; set; }
        [Column("is_deleted")]
        [Precision(1)]
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(DocTableId))]
        public virtual Table Table { get; set; }
        [ForeignKey(nameof(EmpAppointOrderTypeId))]
        public virtual EmpAppointOrderType EmpAppointOrderType { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(EmploymentTypeId))]
        public virtual EmploymentType EmploymentType { get; set; }
        [ForeignKey(nameof(EndTableId))]
        public virtual Table EndTable { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position Position { get; set; }
        [ForeignKey(nameof(WorkScheduleId))]
        public virtual WorkSchedule WorkSchedule { get; set; }
        public virtual ICollection<NeedChamberService> NeedChamberServices { get; set; }
    }
}
