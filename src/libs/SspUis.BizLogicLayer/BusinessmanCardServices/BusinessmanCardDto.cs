using SspUis.BizLogicLayer.ApplicationServices;
using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessmanCardServices
{
    public class BusinessmanCardDto
    {
        public DavAktivContractorDto davAktivContractor { get; set; } = new DavAktivContractorDto();
        public SoliqContractorByTinDto soliqContractorByTin { get; set; } = new SoliqContractorByTinDto();
        public TadbirkorFundContractorDto tadbirkorFundContractor { get; set; } = new TadbirkorFundContractorDto();
        public List<MarkaziyBankContractorCreditHistoryDto> markaziyBankContractorCreditHistory{ get; set; } = new List<MarkaziyBankContractorCreditHistoryDto>();
        public SoliqContractorDebtByTinDataDto soliqContractorDebtByTin { get; set; } = new SoliqContractorDebtByTinDataDto();
        public IntegrationCertificateRequestDto integrationCertificateDto { get; set; } = new IntegrationCertificateRequestDto();
        public SoliqContractorEmployeeCountByTinDataDto soliqContractorEmployeeCountByTin { get; set; } = new SoliqContractorEmployeeCountByTinDataDto { };
        public SoliqContractorFinanceBenefitByTinDataDto soliqContractorFinanceBenefitByTinLastYear { get; set; } = new SoliqContractorFinanceBenefitByTinDataDto() { };
        public SoliqContractorFinanceBenefitByTinDataDto soliqContractorFinanceBenefitByTinThisYear { get; set; } = new SoliqContractorFinanceBenefitByTinDataDto() { };
        public List<GetGTDByInnDataDto> getGTDByInn { get; set; } = new List<GetGTDByInnDataDto> { };
        public CompanyInfo companyInfo { get; set; } = new CompanyInfo();
    }
}
