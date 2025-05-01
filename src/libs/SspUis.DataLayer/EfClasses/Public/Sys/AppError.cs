using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StatusGeneric;
using WEBASE.Models;
using WEBASE;

#nullable disable

namespace SspUis.DataLayer.EfClasses
{
    [Table("sys_app_error")]
    public partial class AppError : IHaveIdProp<long>
    {
        public AppError()
        {
        }

        [Key]
        [Column("id")]
        public long Id { get; set; }
        [Column("request_trace_id")]
        public string? RequestTraceId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("user_name")]
        public string UserName { get; set; }
        [Column("host")]
        public string Host { get; set; } = null!;
        [Column("request_path")]
        public string RequestPath { get; set; } = null!;
        [Column("request_params")]
        public string? RequestParams { get; set; }
        [Column("request_body")]
        public string RequestBody { get; set; }
        [Column("status_code")]
        public int StatusCode { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("detail")]
        public string Detail { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("ip_address")]
        [StringLength(200)]
        public string IpAddress { get; set; }
        [Column("user_agent")]
        [StringLength(2000)]
        public string UserAgent { get; set; }
        [Column("created_at", TypeName = "timestamp without time zone")]
        public DateTime CreatedAt { get; set; }
    }
}
