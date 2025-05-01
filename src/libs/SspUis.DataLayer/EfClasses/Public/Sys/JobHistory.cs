using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_job_history")]
    public partial class JobHistory : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("organization_id")]
        public int? OrganizationId { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("start_at", TypeName = "timestamp without time zone")]
        public DateTime StartAt { get; set; }
        [Column("end_at", TypeName = "timestamp without time zone")]
        public DateTime? EndAt { get; set; }
        [Column("message")]
        public string Message { get; set; }

        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
    }
}
