namespace SspUis.Core.Configurations
{
    public class SystemConf
    {
        public int DefaultOrganizationId { get; set; }
        public string WordTemplatesFolder { get; set; }
        public int AttestationCertificateExpiration { get; set; } //Months
        public string BoDomain { get; set; }
        public string InspectorDomain { get; set; }
        public bool CheckForDomain { get; set; } = false;
        public string LetterVersion { get; set; } = "v01";
        public string QrImagePrintPath { get; set; }
        public string QrImagePrintMy { get; set; }
        public string QrImagePrintERP { get; set; }
        public bool IsTest { get; set; }
        public bool IsLocalHost { get; set; }
    }
}
