namespace SspUis.BizLogicLayer.ReportServices
{
    public class MemshipContractPaidDto
    {
        public int? RegionalOrganizationId { get; set; }
        public string RegionalOrganization { get; set; }
        public string RegionalOrganizationOrderCode { get; set; }

        public long? ContractorId { get; set; }
        public string Contractor { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorPhoneNumber { get; set; }

        public long TotalMemshipContractCount { get; set; }
        public decimal TotalAmount { get; set; }
        public long TotalPaidContractCount { get; set; }
        public decimal TotalPaidFromMemshimpContractAmount { get; set; }
        public decimal TotalPaymentAmount { get; set; }
        public decimal TotalTermPaymentAmount { get; set; }
        public decimal TotalNotPaidAmount { get; set; }
        public decimal TotalUnpaidOnTimeAmount { get; set; }
        public decimal TotalPaymentInDue { get; set; }
    }
}
