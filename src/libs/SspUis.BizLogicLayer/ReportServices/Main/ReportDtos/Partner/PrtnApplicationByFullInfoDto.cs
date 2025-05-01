using System;

namespace SspUis.BizLogicLayer
{
    public class PrtnApplicationByFullInfoDto
    {
        public string ContractorRegion { get; set; }
        public string IsRegion { get; set; }
        public string Region { get; set; }
        public string District { get; set; }
        public string Organization { get; set; }
        public string MFY { get; set; }
        public string INN { get; set; }
        public DateTime? DateSendedOfApplication { get; set; }
        public DateTime? DateApplicationSigned {  get; set; }
        public int DaysLateToApplicationSign { get; set; }
        public DateTime? DateOfExpertOpinion { get; set; }
        public int DateLateToExpertOpinion { get; set; }
        public DateTime? DateSignedByBusinessman { get; set; }
        public DateTime? DateOfOrganization1 { get; set; }
        public DateTime? DateOfOrganization2 { get; set; }
        public int DateLateToOrganization2 { get; set; }
        public DateOnly? DateOfCertificate { get; set; }
    }
}
    