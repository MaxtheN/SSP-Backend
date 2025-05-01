using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsLogListDto : ILinkToEntity<SendSmsLog>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public string PhoneNumer { get; set; }
        public string SmsText { get; set; }
        public string? ErrorText { get; set; }
        public int TableId { get; set; }
        public string Table { get; set; }
        public int? FromStatusId { get; set; }
        public string FromStatus { get; set; }
        public int? ToStatusId { get; set; }
        public string ToStatus{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
