using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;

namespace SspUis.Integration.IntegrationCertificateMB
{
    public interface IIntegrationCertificateMBService :
        IStatusGeneric
    {
        Task<IntegrationCertificateResponseDto> PostCertificateMB(IntegrationCertificateRequestDto dto);
    }
}