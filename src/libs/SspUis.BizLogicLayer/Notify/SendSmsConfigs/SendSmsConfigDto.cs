using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.Notify
{
    public class SendSmsConfigDto : UpdateSendSmsConfigDlDto, ILinkToEntity<SendSmsConfig>
    {
        public string Table { get; set; }
        public string FromStatus { get; set; }
        public string ToStatus { get; set; }
        public string State { get; set; }
    }
}
