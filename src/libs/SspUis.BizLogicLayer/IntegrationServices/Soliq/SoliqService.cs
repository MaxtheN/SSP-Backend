using System.Collections.Generic;
using System.Threading.Tasks;
using SspUis.Integration.Soliq;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;

namespace SspUis.BizLogicLayer.IntegrationServices.Soliq
{
    public class SoliqService : StatusGenericHandler, ISoliqService
    {
        private readonly ISoliqContractorService _contractorService;

        public SoliqService(ISoliqContractorService contractorService)
        {
            _contractorService = contractorService;
        }

        public async Task<CompanyInfo> GetCompanyCriteries(string tin)
        {
            var res = await _contractorService.GetCompanyCriteries(tin);

            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;

            return res;
           
        }

        public async Task<List<GetCompanyHighNewInfo>> GetCompanyHighNewInfo(int isBusiness, int ns10Code, int ns11Code)
        {
           var res = await _contractorService.GetCompanyHighNewInfo(isBusiness, ns10Code, ns11Code);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;

            return res;

        }

        public async Task<List<CompanyStateNewInfoData>> GetCompanyStateNewInfoData(int isBusiness, int ns10Code, int ns11Code)
        {
            var res = await _contractorService.GetCompanyStateNewInfoData(isBusiness, ns10Code, ns11Code);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;

            return res;
        }

        public async Task<SoliqQqsAylanmaData> GetQqsAylanmaData(int month, int inn, int year)
        {
            var res = await _contractorService.GetQqsAylanmaData(month, inn, year);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;

            return res;
        }

        public async Task<SoliqAosAylanmaData> GetAosAylanmaData(int inn, int year)
        {
            var res = await _contractorService.GetAosAylanmaData(inn:inn,year: year,month: null);
            CombineStatuses(_contractorService);
            if (HasErrors)
                return null;

            return res;
        }

        public async Task<SoliqImtiyozResponseDto> GetSoliqImtiyozlari(decimal STIR, int year) //
        {
            var res = await _contractorService.GetSoliqImtiyozlari(STIR, year);
            return res;
        }
    }
}
