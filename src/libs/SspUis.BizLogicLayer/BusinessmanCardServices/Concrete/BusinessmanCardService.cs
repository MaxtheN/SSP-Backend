using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.DataLayer;
using SspUis.Integration.Bojxona.Models;
using SspUis.Integration.Bojxona.Services;
using SspUis.Integration.DavAktiv;
using SspUis.Integration.MarkaziyBank;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using SspUis.Integration.TadbirkorFund;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.BusinessmanCardServices
{
    public class BusinessmanCardService :
        StatusGenericHandler,
        IBusinessmanCardService
    {
        private readonly ISoliqContractorService _soliqContractorService;
        private readonly IDavAktivContractorService _davAktivContractorService;
        private readonly ITadbirkorFundContractorService _tadbirkorFundContractorService;
        private readonly IMarkaziyBankContractorService _markaziyBankContractorService;
        private readonly IPrtnCertificateService _prtnCertificateService;
        private readonly IBojxonaService _bojxonaService;
        private readonly IUnitOfWork _unitOfWork;

        public BusinessmanCardService(
            ISoliqContractorService soliqContractorService,
            IDavAktivContractorService davAktivContractorService,
            IPrtnCertificateService prtnCertificateService,
            IBojxonaService bojxonaService,
            IUnitOfWork unitOfWork
,
            ITadbirkorFundContractorService tadbirkorFundContractorService,
            IMarkaziyBankContractorService markaziyBankContractorService)
        {
            _soliqContractorService = soliqContractorService;
            _davAktivContractorService = davAktivContractorService;
            _prtnCertificateService = prtnCertificateService;
            _bojxonaService = bojxonaService;
            _unitOfWork = unitOfWork;
            _tadbirkorFundContractorService = tadbirkorFundContractorService;
            _markaziyBankContractorService = markaziyBankContractorService;
        }

        public async Task<BusinessmanCardDto> GetByInn(string inn)
        {
            inn = inn.Trim();

            var result = new BusinessmanCardDto();

            //Soliq Ma'lumot
            var soliqInfo = await _soliqContractorService.GetByInn(inn);
            result.soliqContractorByTin = soliqInfo ?? new SoliqContractorByTinDto { };

            //DavAktivContractor ma'lumot
            var davAktivContractorInfo = await _davAktivContractorService.GetByInn(inn);
            result.davAktivContractor = davAktivContractorInfo ?? new DavAktivContractorDto { };

            //TadbirkorFund ma'lumot
            //var tadbirkorFundContractorInfo = await _tadbirkorFundContractorService.GetByInn(inn);
            //result.tadbirkorFundContractor = tadbirkorFundContractorInfo ?? new TadbirkorFundContractorDto { };

            //MarkaziyBankCreditHistory ma'lumot
            var markaziyBankContractorCreditHistoryInfo = await _markaziyBankContractorService.GetCreditHistoryByInn(inn, "2010-01-01", "2050-01-01");
            result.markaziyBankContractorCreditHistory = markaziyBankContractorCreditHistoryInfo ?? new List<MarkaziyBankContractorCreditHistoryDto> { };

            var prtnCertificateInfo = _prtnCertificateService.GetCertificateInfo(ServiceProvider.CultureHelper.CurrentCulture.Id, null, inn);
            result.integrationCertificateDto = prtnCertificateInfo ?? new IntegrationCertificateRequestDto { };

            //Soliqdan qarz
            var soliqContractorDebtByTinInfo = _soliqContractorService.GetDebtByInn(inn, DateTime.Now.Year);
            result.soliqContractorDebtByTin = await soliqContractorDebtByTinInfo ?? new SoliqContractorDebtByTinDataDto { };

            //Soliq employeeCount
            var soliqContractorEmployeeCountByTinInfo = _soliqContractorService.GetEmployeeCountByInn(inn, DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            result.soliqContractorEmployeeCountByTin = await soliqContractorEmployeeCountByTinInfo ?? new SoliqContractorEmployeeCountByTinDataDto { };

            //Soliq Foyda
            var soliqContractorFinanceBenefitByTinInfo = _soliqContractorService.GetFinanceBenefitByInn(inn, DateTime.Now.AddMonths(-1).Year, ToPeriod(DateTime.Now.AddMonths(-1).Month));
            result.soliqContractorFinanceBenefitByTinThisYear = await soliqContractorFinanceBenefitByTinInfo ?? new SoliqContractorFinanceBenefitByTinDataDto();

            soliqContractorFinanceBenefitByTinInfo = _soliqContractorService.GetFinanceBenefitByInn(inn, DateTime.Now.AddYears(-1).Year, 4);
            result.soliqContractorFinanceBenefitByTinLastYear = await soliqContractorFinanceBenefitByTinInfo ?? new SoliqContractorFinanceBenefitByTinDataDto();

            //GTD
            var getGTDByInnInfo = _bojxonaService.GetGTDByInn(new GetGTDByInnRequestDto
            {
                Stir = inn,
                Year = DateTime.Now.Year.ToString(),
            });
            result.getGTDByInn = await getGTDByInnInfo ?? new List<GetGTDByInnDataDto>();

            var companyCriteriesInfo = _soliqContractorService.GetCompanyCriteries(inn);
            result.companyInfo = await companyCriteriesInfo ?? new CompanyInfo();

            //CombineStatuses(_soliqContractorService);
            //CombineStatuses(_davAktivContractorService);
            //CombineStatuses(_prtnCertificateService);
            //CombineStatuses(_tadbirkorFundContractorService);
            //CombineStatuses(_markaziyBankContractorService);

            return result;
        }

        private int ToPeriod(int month)
        {

            if (month > 0 && month <= 3)
            {
                return 1;
            }
            else if (month > 3 && month <= 6)
            {
                return 2;
            }
            else if (month > 6 && month <= 9)
            {
                return 3;
            }
            else if (month > 9 && month <= 12)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }
    }
}
