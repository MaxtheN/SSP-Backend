namespace SspUis.BizLogicLayer.Doc.ApplicationServices
{
    public class ApplicationVacanсiesDto
    {
        public int NewVacanciesCount { get; set; }
        public decimal ReportVacanciesCount { get; set; }
    }

    public class MemshipPaymentsInfo
    {
        public string Payment { get; set; }
        public string PaymentPayment { get; set; }
    }

    public class AvailableBenefits
    {
        public string OrganizationName { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public decimal? Sum { get; set; }
    }
}
