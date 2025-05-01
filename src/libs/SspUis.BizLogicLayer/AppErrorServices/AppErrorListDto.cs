using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.AppErrorServices
{
    public class AppErrorListDto : ILinkToEntity<AppError>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string? RequestTraceId { get; set; }
        public string RequestPath { get; set; } = null!;
        public int StatusCode { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Host { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserAgent { get; set; }
    }

}
