using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocumentHistoryListDto : ILinkToEntity<DocumentHistory>//, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public DateTime DateAt { get; set; }
        public int UserId { get; set; }
        public string UserInfo { get; set; } = null!;
        public int TableId { get; set; }
        public long DocId { get; set; }
        public int StatusId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? Message { get; set; }
        public int OrganizationId { get; set; }
        public string DocContent { get; set; } = null!;
    }
}
