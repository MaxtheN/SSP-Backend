
using StatusGeneric;

namespace SspUis.Integration.OneId;

public interface IOneIdService : IStatusGenericHandler
{
    Task<AccessTokenResponseDto?> GetAccessToken(string code, string redirectUrl);
    Task Logout(string accessToken);
    Task<OneIdUserDataDto?> GetUserData(string accessToken);
}