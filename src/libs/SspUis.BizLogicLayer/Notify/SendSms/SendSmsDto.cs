namespace SspUis.BizLogicLayer
{
    public class SendSmsDto
    {
        public int TableId { get; set; }
        public int? FromStatusId { get; set; }
        public int? ToStatusId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
