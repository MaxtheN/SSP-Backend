namespace SspUis.BizLogicLayer.DocumentHistoryService
{
    public class DocLastMessageResponseDto
    {
        public int? PreviaousStatusId { get; set; }
        public int CurrenctStatusId { get; set; }
        public string Message { get; set; }
    }
}
