using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.DocumentHistoryServices
{
    public class DocumentJobHistoryListDto : ILinkToEntity<DocumentJobHistory>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public int TableId { get; set; }
        public string Table { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int FromStatusId { get; set; }
        public string FromStatus { get; set; }
        public int ToStatusId { get; set; }
        public string ToStatus { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
    }
}
