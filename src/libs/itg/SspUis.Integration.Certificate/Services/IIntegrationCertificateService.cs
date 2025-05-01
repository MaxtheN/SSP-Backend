using SspUis.BizLogicLayer.PrtnCertificateServices;
using StatusGeneric;

namespace SspUis.Integration.IntegrationCertificate
{
    public interface IIntegrationCertificateService :
        IStatusGeneric
    {
        Task<IntegrationCertificateResponseDto> PostCertificate(IntegrationCertificateRequestDto dto);
    }
}