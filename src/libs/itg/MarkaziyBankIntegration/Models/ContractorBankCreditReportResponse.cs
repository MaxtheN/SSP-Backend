namespace SspUis.Integration.BankCredit.Models
{
    public class ContractorBankCreditReportResponse
    {
        public string ContractorInn { get; set; }
        public string Contractor { get; set; }
        public int Year { get; set; }
        public string BankMfo { get; set; }
        public string BankName { get; set; }
        public int? RegionId { get; set; }
        public string RegionName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string BankCodeFullName { get; set; }
        public ContractorContractType Application { get; set; }
        public ContractorContractType ContractType1 { get; set; }
        public ContractorContractType ContractType2 { get; set; }
        public ContractorContractType ContractType3 { get; set; }
    }
    public class ContractorContractType
    {
        public int ApprovedCount { get; set; }
        public double ApprovedSum { get; set; }
        
        public double SubmittedCount { get; set; }
        public double SubmittedSum { get; set; }
        
        public int IssuanceCount { get; set; }
        public double IssuanceSum { get; set; }
        
        public int CanceledCount { get; set; }
        public double CanceledSum { get; set; }
        
        public int RejectedCount { get; set; }
        public double RejectedSum { get; set; }
    }
}