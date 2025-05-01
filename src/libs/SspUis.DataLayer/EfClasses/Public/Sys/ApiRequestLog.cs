using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_api_request_log")]
    [Index(nameof(TableId), nameof(DocumentId), Name = "ix_sys_api_request_log__table_id_doc_id")]
    public partial class ApiRequestLog : IHaveIdProp<Guid>
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("is_success")]
        public bool IsSuccess { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("user_info", TypeName = "json")]
        public string? UserInfo { get; set; }
        [Column("table_id")]
        public int? TableId { get; set; }
        [Column("document_id")]
        public long? DocumentId { get; set; }
        [Column("request_url")]
        [StringLength(500)]
        public string? RequestUrl { get; set; }
        [Column("request_content", TypeName = "json")]
        public string? RequestContent { get; set; }
        [Column("response_status")]
        public int? ResponseStatus { get; set; }
        [Column("response_content", TypeName = "character varying")]
        public string? ResponseContent { get; set; }
        [Column("result", TypeName = "json")]
        public string? Result { get; set; }
        [Column("exception", TypeName = "character varying")]
        public string? Exception { get; set; }
        [Column("request_at", TypeName = "timestamp without time zone")]
        public DateTime RequestAt { get; set; }
        [Column("response_at", TypeName = "timestamp without time zone")]
        public DateTime? ResponseAt { get; set; }
    }
}
