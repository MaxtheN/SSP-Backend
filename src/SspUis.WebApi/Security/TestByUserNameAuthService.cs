using Microsoft.EntityFrameworkCore;
using WEBASE.AspNet.Security;

namespace SspUis.WebApi.Security;

public class TestByUserNameAuthService : AuthService
{
    private string _userName;

    public TestByUserNameAuthService(string userName, IHttpContextAccessor httpContextAccessor, DbContext dbContext, JwtConfig jwtConfig)
        : base(httpContextAccessor, dbContext, jwtConfig)
    {
        _userName = userName;
    }

    public override bool IsAuthenticated => true;
    public override string UserName => _userName;
}
