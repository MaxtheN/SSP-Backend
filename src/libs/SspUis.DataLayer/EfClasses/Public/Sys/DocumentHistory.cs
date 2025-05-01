using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_document_history")]
    [Index(nameof(TableId), nameof(DocId), Name = "ix_sys_document_history__table_doc")]
    public partial class DocumentHistory : IHaveIdProp<long>
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("date_at", TypeName = "timestamp without time zone")]
        public DateTime DateAt { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("user_info", TypeName = "json")]
        public string UserInfo { get; set; }
        [Column("table_id")]
        public int TableId { get; set; }
        [Column("doc_id")]
        public long DocId { get; set; }
        [Column("status_id")]
        public int StatusId { get; set; }
        [Column("ip_address")]
        [StringLength(50)]
        public string IpAddress { get; set; }
        [Column("user_agent")]
        [StringLength(250)]
        public string UserAgent { get; set; }
        [Column("message")]
        [StringLength(500)]
        public string Message { get; set; }
        [Column("organization_id")]
        public int OrganizationId { get; set; }
        [Required]
        [Column("doc_content", TypeName = "json")]
        public string DocContent { get; set; }
        [Column("change_log_id")]
        public long ChangeLogId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
        [ForeignKey(nameof(TableId))]
        public virtual Table Table { get; set; }
    }
}
