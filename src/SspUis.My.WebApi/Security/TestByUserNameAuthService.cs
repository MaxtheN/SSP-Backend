using SspUis.Core.Security;
using SspUis.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.AspNet.Security;
using WEBASE.EF;

namespace SspUis.My.WebApi.Security
{
    public class TestByUserNameAuthService : AuthService
    {
        private string _userName;

        public TestByUserNameAuthService(string userName, IHttpContextAccessor httpContextAccessor, DbContext dbContext, CookieConfig cookieConfig)
            : base(httpContextAccessor, dbContext, cookieConfig)
        {
            _userName = userName;
        }

        public override bool IsAuthenticated => true;
        public override string UserName => _userName;
    }
}
