using WEBASE.AspNet.Security;

namespace SspUis.Core.Security
{
    public interface IIntegrationAuthService : ICookieJwtAuthService
    {
        IntegrationUserAuthModel User { get; }
    }
}
