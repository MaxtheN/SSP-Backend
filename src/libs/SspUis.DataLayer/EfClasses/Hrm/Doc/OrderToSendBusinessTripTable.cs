using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("doc_order_to_send_business_trip_table", Schema = "hrm")]
    public partial class OrderToSendBusinessTripTable : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("owner_id")]
        public long OwnerId { get; set; }
        [Column("begin_on")]
        public DateOnly BeginOn { get; set; }
        [Column("end_on")]
        public DateOnly EndOn { get; set; }
        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }
        [Column("department_id")]
        public int DepartmentId { get; set; }
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }
        [Column("country_id")]
        public int? CountryId { get; set; }
        [Column("detail_for_print")]
        public string DetailForPrint { get; set; }
        [Column("region_id")]
        public int? RegionId { get; set; }
        [Column("temp_employee_manage_id")]
        public long? TempEmployeeManageId { get; set; }
        [Column("business_trip_type_id")]
        public int BusinessTripTypeId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("another_organization")]
        [StringLength(250)]
        public string AnotherOrganization { get; set; }

        [Column("work_start_date")]
        public DateOnly? WorkStarDate { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty(nameof(OrderToSendBusinessTrip.Tables))]
        public virtual OrderToSendBusinessTrip Owner { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }
        [ForeignKey(nameof(BusinessTripTypeId))]
        public virtual OrderToSendBusinessTripType BusinessTripType { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organization { get; set; }
    }
}
