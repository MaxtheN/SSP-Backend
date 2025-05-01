using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.Core.Configurations;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.My.WebApi;
using SspUis.My.WebApi.Security;
using WEBASE.AspNet.Security;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.WebApi.Security
{

    public class IntegrationAuthService : JwtAuthService, IIntegrationAuthService
    {
        private IntegrationAuthConfig _jwtAuthConfig;

        public IntegrationAuthService(
            IHttpContextAccessor httpContextAccessor,
            IntegrationAuthConfig jwtAuthConfig)
            : base(jwtAuthConfig.Jwt, httpContextAccessor)
        {
            _jwtAuthConfig = jwtAuthConfig;
        }

        private IntegrationUserAuthModel _user;
        public IntegrationUserAuthModel User
        {
            get
            {
                if (IsAuthenticated && _user == null)
                {
                    _user = _jwtAuthConfig.IntegrationUsers
                        .Where(a=> a.UserName == UserName)
                        .MapToAuthModel()
                        .FirstOrDefault();
                }

                return _user;
            }
        }
        public override string UserName => base.UserName;
        public override object UserId { get; }
        public override HashSet<string> Modules { get; }
        public string Login(string userName)
        {
            throw new NotImplementedException();
        }
        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}