using SspUis.BizLogicLayer.BusinessmanAccountServices;
using SspUis.Core;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.AspNet.Security;
using WEBASE.EF;
using WEBASE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WEBASE;

namespace SspUis.My.WebApi.Security
{
    public class AuthService : CookieJwtAuthService, IAuthService
    {
        private readonly DbContext _context;

        public AuthService(IHttpContextAccessor httpContextAccessor,
                           DbContext dbContext,
                           CookieConfig cookieConfig)
            : base(cookieConfig, httpContextAccessor)
        {
            _context = dbContext;
            ((BaseDbContext)_context).SetAuthService(this);
        }

        private UserAuthModel _user;
        public UserAuthModel User
        {
            get
            {
                if (IsAuthenticated && _user == null)
                {
                    _user = _context.Set<BusinessmanUser>()
                                    .Where(a => a.UserName == UserName)
                                    .MapToAuthModel()
                                    .FirstOrDefault();
                    _user?.ResolveModules();
                }

                return _user;
            }
        }


        private int? _userId;
        public override object UserId
        {
            get
            {
                if (_userId != null)
                    return _userId;

                if (IsAuthenticated)
                    _userId = this.User?.Id;

                return _userId;
            }
        }

        private HashSet<string> _modules;
        public override HashSet<string> Modules
        {
            get
            {
                if (IsAuthenticated && _modules == null)
                    _modules = User.Modules.ToHashSet();
                return _modules;
            }
        }

        public bool HasPermission(params ModuleCode[] moduleCodes)
        {
            return moduleCodes.Any(a => HasPermission(a.ToString()));
        }

        private ContractorAuthModel? _contractor = null;
        public ContractorAuthModel Contractor
        {
            get
            {
                if (_contractor == null)
                {
                    var contractorIdString = HttpContextAccessor.HttpContext.Request.Cookies["contractor-id"];
                    if (!string.IsNullOrEmpty(contractorIdString))
                    {
                        var contractorId = long.Parse(contractorIdString);
                        var query = _context.Set<Contractor>().Where(a => a.Id == contractorId).MapToAuthModel();
                        _contractor = query.FirstOrDefault();
                    }
                }
                return _contractor;
            }
        }
        private bool _oneIdTokenIsInitialized = false;
        private string _oneIdToken;
        public string OneIdToken
        {
            get
            {
                if (!_oneIdToken.NullOrEmpty() || _oneIdTokenIsInitialized)
                    return _oneIdToken;
                if (IsAuthenticated)
                    _oneIdToken = new JwtSecurityTokenHandler().ReadJwtToken(ReadTokenFromRequest()).Claims.FirstOrDefault((Claim a) => a.Type == "one-token")?.Value;
                _oneIdTokenIsInitialized = true;
                return _oneIdToken;
            }
        }

        public OrganizationAuthModel Organization => throw new NotImplementedException();

        protected override void Clear()
        {
            base.Clear();
            _user = null;
            _modules = null;
            _userId = null;
            _contractor = null;
        }

        public override void Logout()
        {
            HttpContextAccessor.HttpContext.Response.Cookies.Delete("contractor-id", new CookieOptions
            {
                HttpOnly = true,
                Domain = Config.Domain
            });
            base.Logout();
        }

        public void SelectContractor(long contractorId)
        {
            if (HttpContextAccessor.HttpContext.Request.Cookies.ContainsKey("contractor-id"))
            {
                HttpContextAccessor.HttpContext.Response.Cookies.Delete("contractor-id", new CookieOptions
                {
                    HttpOnly = true,
                    Domain = Config.Domain
                });
            }

            HttpContextAccessor.HttpContext.Response.Cookies.Append("contractor-id", $"{contractorId}", new CookieOptions
            {
                HttpOnly = true,
                Domain = Config.Domain,
                Expires = DateTimeOffset.Now.AddMinutes(Config.Expires)
            });

            _contractor = _context.Set<Contractor>().Where(a => a.Id == contractorId).MapToAuthModel().FirstOrDefault();
        }
    }
}
