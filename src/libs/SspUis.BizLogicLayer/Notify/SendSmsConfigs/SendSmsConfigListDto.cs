using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsConfigListDto : ILinkToEntity<SendSmsConfig>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SmsText { get; set; }
        public int TableId { get; set; }
        public string Table { get; set; }
        public int? FromStatusId { get; set; }
        public string FromStatus { get; set; }
        public int? ToStatusId { get; set; }
        public string ToStatus { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
