using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class AppErrorDlDto<TDto> : EntityDto<TDto, AppError>
        where TDto : AppErrorDlDto<TDto>
    {
        public long Id { get; set; }
        public string? RequestTraceId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; } = null!;
        public string RequestPath { get; set; } = null!;
        public string? RequestParams { get; set; }
        public string RequestBody { get; set; }
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}
