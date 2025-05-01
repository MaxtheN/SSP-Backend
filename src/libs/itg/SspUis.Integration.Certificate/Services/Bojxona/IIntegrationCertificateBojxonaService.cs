using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;

namespace SspUis.Integration.IntegrationCertificateMB
{
    public interface IIntegrationCertificateBojxonaService :
        IStatusGeneric
    {
        Task<IntegrationCertificateResponseDto> PostCertificateBojxona(IntegrationCertificateRequestDto dto);
    }
}