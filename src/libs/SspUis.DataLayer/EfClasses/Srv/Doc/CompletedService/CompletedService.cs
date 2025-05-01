using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_completed_service", Schema = "srv")]
    public class CompletedService : IHaveIdProp<long>, IHaveStatusId
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [StringLength(30)]
        [Column("doc_number")]
        public string DocNumber { get; set; }

        [Required]
        [Column("doc_on")]
        public DateOnly DocOn { get; set; }

        [Column("details")]
        [StringLength(600)]
        public string Details { get; set; }

        [Required]
        [Column("contractor_id")]
        public long ContractorId { get; set; }

        [Column("service_contract_id")]
        public long? ServiceContractId { get; set; }

        [Column("service_application_id")]
        public long? ServiceApplicationId { get; set; }

        [Required]
        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }

        [ForeignKey(nameof(ServiceContractId))]
        public virtual ServiceContract ServiceContract { get; set; }

        [ForeignKey(nameof(ServiceApplicationId))]
        public virtual ServiceApplication ServiceApplication { get; set; }

        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
    }
}
