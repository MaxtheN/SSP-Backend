using SspUis.Integration.BankCredit.Models;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class BankCreditReportDto
    {
        public int Year { get; set; }

        public string BankMfo { get; set; }

        public string BankName { get; set; }

        public int RegionId { get; set; }

        public string RegionName { get; set; }

        public int DistrictId { get; set; }

        public string DistrictName { get; set; }

        public ContractType Application { get; set; }

        public ContractType ContractType1 { get; set; }

        public ContractType ContractType2 { get; set; }

        public ContractType ContractType3 { get; set; }
    }
}
