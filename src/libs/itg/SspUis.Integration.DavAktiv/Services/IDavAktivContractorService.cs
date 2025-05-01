using StatusGeneric;

namespace SspUis.Integration.DavAktiv
{
    public interface IDavAktivContractorService : IStatusGeneric
    {
        Task<DavAktivContractorDto> GetByInn(string inn);
    }
}