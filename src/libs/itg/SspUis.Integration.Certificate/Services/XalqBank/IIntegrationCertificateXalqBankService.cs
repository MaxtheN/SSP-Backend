using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;

namespace SspUis.Integration.IntegrationCertificateXalqBank
{
    public interface IIntegrationCertificateXalqBankService :
        IStatusGeneric
    {
        Task<IntegrationCertificateResponseDto> PostCertificateXalqBank(IntegrationCertificateRequestDto dto);
    }
}