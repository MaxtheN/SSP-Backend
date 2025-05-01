using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Partner
{
    public class PrtnReportsDashTotalStatisticsDto
    {
        public long FormedCertificateCount { get; set; }
        public long BankLoanSeparatorsCount { get; set; }
        public long TookAdvantegTaxDeductoinCount { get; set; }
        public long TookAdvantegTaxDeductoinSumma{ get; set; }
        public long CustomsPrivilegeBeneficiariesCount { get; set; }
        public long CustomsPrivilegeBeneficiariesSumma { get; set; }
        public long TookAdvantegPrivilegeLeasingAssetsCount { get; set; }
        public long TookAdvantegPrivilegeLeasingAssetsSumma { get; set; }
        public long ReceivedBailEnteepreneurialFundCount { get; set; }
        public long ReceivedBailEnteepreneurialFundSumma { get; set; }
    }
    public class CustomsPrivilegeDto : DashboardFilterOption
    {
        public long GrnChanContractorCount { get; set; }
        public long AppContractorCount { get; set; }
        public long RejContractorCount { get; set; }
        public long DevContractorCount { get; set; }
        public long SummaInstallment { get; set; }
    }
    public class PrtnEntepreneurialDashDto : DashboardFilterOption
    {
        public string PrtnContractType { get; set; }
        public decimal Amount { get; set; }
        public decimal Summa { get; set; }
    }
    public class CreditsDto : DashboardFilterOption
    {
        public long SubmittedCount { get; set; }
        public decimal SubmittedSumma { get; set; }
        public long RejectedApplicationCount { get; set; }
        public decimal RejectedApplicationSumma { get; set; }
        public long CanceledApplicationCount { get; set; }
        public decimal CanceledApplicationSumma { get; set; }
        public long ApprovedApplicationCount { get; set; }
        public decimal ApprovedApplicationSumma { get; set; }
        public long LoanAllocationApplicationCount { get; set; }
        public decimal LoanAllocationApplicationSumma { get; set; }
    }
    public class StateAssetDashDto : DashboardFilterOption
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }
    public class PrtnTaxPrivilegeRepotsDashDto
    {
        public int PrtnContractTypeId { get; set; }
        public string PrtnContractType { get; set; }
        public List<PrtnTaxCreditClomn> PrtnLandTax { get; set; } = new();
        public List<PrtnTaxCreditClomn> PrtnPropertyTax { get; set; } = new();
        public List<PrtnTaxCreditClomn> PrtnSocial { get; set; } = new();
        public List<PrtnTaxCreditClomn> PrtnIncomeTax { get; set; } = new();
    }
    public class PrtnTaxCreditClomn
    {
        public long Count { get; set; }
        public long Summa { get; set; }
    }
    public class PrtnReportsDashRateDto : RegionRateFilterOption
    {
        public string RegionOrderCode { get; set; }
        public string DistrictOrderCode { get; set; }
        public string MfyOrderCode { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal Summa { get; set; }
    }
    public static class RegionConstForCredit
    {
        public static Dictionary<int, int> Dict { get; } = new Dictionary<int, int>()
        {
            {1, 26}, {2, 27}, {4, 6},{5, 8}, {6, 35}, {7, 10},{8, 12}, {9, 14}, {10, 18},{11, 22}, {12, 24}, {13, 30},{14, 33}
        };
    }
}