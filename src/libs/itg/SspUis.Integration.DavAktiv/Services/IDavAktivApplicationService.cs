using StatusGeneric;

namespace SspUis.Integration.DavAktiv
{
    public interface IDavAktivApplicationService : IStatusGeneric
    {
        Task<(DavAktivApplicationResponseDto result, HttpResponseMessage response, string responseText, string url)> PostApplication(DavAktivApplicationDto dto);
    }
}