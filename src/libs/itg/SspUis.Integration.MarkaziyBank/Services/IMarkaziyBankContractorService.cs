using SspUis.Integration.MarkaziyBank;
using StatusGeneric;

namespace SspUis.Integration.MarkaziyBank
{
    public interface IMarkaziyBankContractorService : IStatusGeneric
    {
        Task<List<MarkaziyBankContractorCreditHistoryDto>> GetCreditHistoryByInn(string inn, string fromDate, string toDate);
    }
}