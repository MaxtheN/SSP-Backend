using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_employee_send_train_table", Schema = "hrm")]
    public partial class EmployeeSendTrainTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly EndOn { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("detail_for_print")]
        public string DetailForPrint { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("another_organization")]
        [StringLength(250)]
        public string AnotherOrganization { get; set; }

        [Column("work_start_date")]
        public DateOnly? WorkStartDate { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(EmployeeSendTrain.Tables))]
        public virtual EmployeeSendTrain Owner { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organization { get; set; }
    }
}
