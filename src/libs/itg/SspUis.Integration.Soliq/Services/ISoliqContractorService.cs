using iText.StyledXmlParser.Node;
using SspUis.DataLayer.EfClasses;
using SspUis.Integration.Soliq.Models;
using StatusGeneric;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static WEBASE.Storage.StaticFileConst;

namespace SspUis.Integration.Soliq
{
    public interface ISoliqContractorService : IStatusGeneric
    {
        Task<SoliqContractorByTinDto> GetByInn(string inn);
        Task<SoliqContractorDebtByPinflDto> GetByPinfl(string pinfl);
        Task<SoliqContractorEmployeeCountByTinDataDto> GetEmployeeCountByInn(string inn, int year, int month);
        Task<SoliqContractorFinanceBenefitByTinDataDto> GetFinanceBenefitByInn(string inn, int year, int period);
        Task<SoliqContractorDebtByTinDataDto> GetDebtByInn(string inn, int year);
        Task<FarmerRefundByInnDataDto> GetFarmerRefundByInn(string inn, int year);
        Task<List<ImtiyozDataByInnDataDto>> GetImtiyozDataByInn(string inn, int year);
        Task<CompanyInfo> GetCompanyCriteries(string tin);
        Task<List<GetCompanyHighNewInfo>> GetCompanyHighNewInfo(int isBusiness, int ns10Code, int ns11Code);
        Task<List<CompanyStateNewInfoData>> GetCompanyStateNewInfoData(int isBusiness, int ns10Code, int ns11Code);
        Task<SoliqQqsAylanmaData> GetQqsAylanmaData(int month, long inn, int year);
        Task<SoliqImtiyozResponseDto> GetSoliqImtiyozlari(decimal STIR, int year);
        Task<SoliqAosAylanmaData> GetAosAylanmaData(int inn, int year, int? month = null);
    }
}
