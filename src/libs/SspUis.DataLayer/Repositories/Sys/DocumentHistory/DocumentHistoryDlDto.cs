using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using GenericServices;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class DocumentHistoryDlDto : EntityDto<DocumentHistoryDlDto, DocumentHistory>, ILinkToEntity<DocumentHistory>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int UserId { get; set; }
        [LocalizedRequired]
        public DateTime DateAt { get; set; }
        [LocalizedRequired]
        public string UserInfo { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int TableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long DocId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public int StatusId { get; set; }
        [LocalizedStringLength(50)]
        public string IpAddress { get; set; }
        [LocalizedStringLength(250)]
        public string UserAgent { get; set; }
        [LocalizedStringLength(500)]
        public string Message { get; set; }
        public int OrganizationId { get; set; }
        [LocalizedRequired]
        public string DocContent { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ChangeLogId { get; set; }
    }
}
