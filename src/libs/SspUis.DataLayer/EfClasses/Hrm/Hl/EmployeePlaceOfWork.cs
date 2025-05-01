using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm
{
    [Table("hl_employee_place_of_work", Schema = "hrm")]
    [Index(nameof(OwnerId), Name = "ix_hl_employee_place_of_work__owner")]
    public partial class EmployeePlaceOfWork : IHaveIdProp<int>
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        [Column("contractor_id")]
        public long? ContractorId { get; set; }
        [Column("position_id")]
        public int? PositionId { get; set; }
        [Required]
        [Column("position_name")]
        [StringLength(2000)]
        public string PositionName { get; set; }
        [Column("department_name")]
        [StringLength(2000)]
        public string DepartmentName { get; set; }
        [Column("addition_id")]
        public long? AdditionId { get; set; }
        [Required]
        [Column("contractor_name")]
        [StringLength(250)]
        public string ContractorName { get; set; }
        [Column("employment_type_id")]
        public int EmploymentTypeId { get; set; }
        [Column("start_on")]
        public DateOnly StartOn { get; set; }
        [Column("end_on")]
        public DateOnly? EndOn { get; set; }
        [Column("is_imported")]
        public bool IsImported { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }
        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }
        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor? Contractor { get; set; }

        [ForeignKey(nameof(EmploymentTypeId))]
        public virtual EmploymentType EmploymentType { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual Employee Owner { get; set; }
        [ForeignKey(nameof(PositionId))]
        public virtual Position? Position { get; set; }
    }
}
