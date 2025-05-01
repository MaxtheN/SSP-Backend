using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.IntegrationServices.Soliq
{
    public interface ISoliqService : IStatusGeneric
    {
        Task<CompanyInfo> GetCompanyCriteries(string tin);
        Task<List<GetCompanyHighNewInfo>> GetCompanyHighNewInfo(int isBusiness, int ns10Code, int ns11Code);
        Task<List<CompanyStateNewInfoData>> GetCompanyStateNewInfoData(int isBusiness, int ns10Code, int ns11Code);
        Task<SoliqQqsAylanmaData> GetQqsAylanmaData(int month, int inn, int year);
        Task<SoliqAosAylanmaData> GetAosAylanmaData(int inn, int year);
        Task<SoliqImtiyozResponseDto> GetSoliqImtiyozlari(decimal STIR, int year);
    }
}
