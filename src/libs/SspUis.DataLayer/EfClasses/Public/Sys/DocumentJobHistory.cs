using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_document_job_history")]
    public partial class DocumentJobHistory : IHaveIdProp<long>
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
        [Column("doc_id")]
        public long DocId { get; set; }
        /// <summary>
        /// Document Current StatusId
        /// </summary>
        [Column("from_status_id")]
        public int FromStatusId { get; set; }
        /// <summary>
        /// Document New StatusId
        /// </summary>
        [Column("to_status_id")]
        public int ToStatusId { get; set; }
        [Column("is_succeed")]
        public bool IsSucceed { get; set; }
        [Column("message")]
        [StringLength(800)]
        public string? Message { get; set; }
        /// <summary>
        /// Job StatusId
        /// </summary>
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
        [Column("start_at", TypeName = "timestamp without time zone")]
        public DateTime? StartAt { get; set; }
        [Column("end_at", TypeName = "timestamp without time zone")]
        public DateTime? EndAt { get; set; }
        [Column("request_trace_id")]
        [StringLength(50)]
        public string RequestTraceId { get; set; }

        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
        [ForeignKey(nameof(OrganizationId))]
        public virtual Organization Organization { get; set; }
        [ForeignKey(nameof(FromStatusId))]
        public virtual Status FromStatus { get; set; }
        [ForeignKey(nameof(ToStatusId))]
        public virtual Status ToStatus { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
