using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_custom_job")]
    public class CustomJob : IHaveIdProp<long>, IJobDocumentEntity
    {
        public CustomJob() 
        {
            Actions = new HashSet<CustomJobAction>();
        }
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("job_type_id")]
        public int JobTypeId { get; set; }

        [Column("is_force_update")]
        public bool IsForceUpdate { get; set; }

        [Column("start_at", TypeName = "timestamp without time zone")]
        public DateTime? StartAt { get; set; }

        [Column("end_at", TypeName = "timestamp without time zone")]
        public DateTime? EndAt { get; set; }

        [Column("backgound_job_id")]
        [MaxLength(100)]
        public string? BackGoundJobId { get; set; }

        [Column("extend_data")]
        public string? ExtendData { get; set; }

        [Column("total_count")]
        public int TotalCount { get; set; }

        [Column("success_count")]
        public int SuccesCount { get; set; }

        [Column("cache_count")]
        public int CacheCount { get; set; }

        [Column("error_count")]
        public int ErrorCount { get; set; }

        [Column("error")]
        public string? Error { get; set; }

        [Column("region_id")]
        public int? RegionId { get; set; }

        [Column("district_id")]
        public int? DistrictId { get; set; }

        [Column("organization_id")]
        public int? OrganizationId { get; set; }

        [Column("status_id")]
        public int StatusId { get; set; }

        [Column("prev_status_id")]
        public int? PrevStatusId { get; set; }

        [Column("table_id")]
        public int TableId { get; set; } = TableIdConst.SYS_CUSTOM_JOB;

        [Column("message")]
        public string Message { get; set; }

        [Column("hangfire_job_id")]
        public string HangfireJobId { get; set; }

        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }

        [Column("created_user_id")]
        public int? CreatedUserId { get; set; }

        [Column("modified_at", TypeName = "timestamp without time zone")]
        public DateTime? ModifiedAt { get; set; }

        [Column("modified_user_id")]
        public int? ModifiedUserId { get; set; }
       

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }

        [ForeignKey(nameof(PrevStatusId))]
        public virtual Status PrevStatus { get; set; }

        [ForeignKey(nameof(RegionId))]
        public virtual Region? Region { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual District? District { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization? Organization { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        public virtual CustomJobType JobType { get; set; }
        [InverseProperty(nameof(CustomJobAction.Owner))]
        public virtual ICollection<CustomJobAction> Actions { get; set; }

    }
}
