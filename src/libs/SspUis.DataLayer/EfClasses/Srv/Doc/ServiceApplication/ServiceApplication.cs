using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("doc_service_application", Schema = "srv")]
    [Index(nameof(ApplicationId), Name = "uc_application_id", IsUnique = true)]
    public class ServiceApplication : IHaveIdProp<long>, IBaseApplicationEntity
    {
        public ServiceApplication()
        {
            Groups = new HashSet<ServiceApplicationGroup>();
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("application_id")]
        public long ApplicationId { get; set; }

        [Column("employee_manage_id")]
        public long? EmployeeManageId { get; set; }

        [Required]
        [Column("region_id")]
        public int RegionId { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Required]
        [Column("is_free")]
        public bool IsFree { get; set; } = false;

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
        [Column("organization_id")]
        public int? OrganisationId { get; set; }
        [Column("to_regional_office")]
        public bool? ToRegionalOffice { get; set; }
        [Column("message")] 
        public string? Message { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region Region { get; set; }

        [ForeignKey(nameof(EmployeeManageId))]
        public virtual EmployeeManage EmployeeManage { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        [ForeignKey(nameof(OrganisationId))]
        public virtual Organization Organization { get; set; }

        [InverseProperty(nameof(ServiceApplicationGroup.Owner))]
        public virtual ICollection<ServiceApplicationGroup> Groups { get; set; }
    }
}