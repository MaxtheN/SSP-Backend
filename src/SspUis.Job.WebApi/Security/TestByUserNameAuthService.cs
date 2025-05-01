using SspUis.DataLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.AspNet.Security;
using WEBASE.EF;

namespace SspUis.Job.WebApi.Security
{
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
}
