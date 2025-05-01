using SspUis.BizLogicLayer.PrtnCertificateServices;
using SspUis.Integration.IntegrationCertificate;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Certificate
{
    public interface IIntegrationCertificateMoliyaService : IStatusGeneric
    {
        Task<IntegrationCertificateResponseDto> PostCertificateMoliya(IntegrationCertificateRequestDto dto);
    }
}
