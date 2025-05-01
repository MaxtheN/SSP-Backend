using StatusGeneric;

namespace SspUis.Integration.TadbirkorFund
{
    public interface ITadbirkorFundContractorService : IStatusGeneric
    {
        Task<TadbirkorFundContractorDto> GetByInn(string inn);
    }
}