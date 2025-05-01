namespace SspUis.BizLogicLayer.ApplicationServices
{
    public class CheckApplicationStatusDto
    {
        public int ApplicationStatusId { get; set; }
        public string ApplicationStatusName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName { get; set; }
        public int CertificateStatusId { get; set; }
        public string CertificateStatusName { get; set; }
    }
}
