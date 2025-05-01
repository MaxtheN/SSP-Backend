using SspUis.BizLogicLayer.AccountServices;
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

namespace SspUis.Job.WebApi.Security
{
    public class AuthService : JwtAuthService, IAuthService
    {
        private DbContext _context;

        public AuthService(IHttpContextAccessor httpContextAccessor, DbContext dbContext, JwtConfig jwtConfig)
            : base(jwtConfig, httpContextAccessor)
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
                    _user = _context.Set<User>()
                                    .Include(a => a.Person)
                                    //.Include(a => a.Employee)
                                    .ByUserName(UserName)
                                    .MapToAuthModel()
                                    .FirstOrDefault();
                    _user?.ResolveModules();
                }

                return _user;
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
        private OrganizationAuthModel _organization;

        public OrganizationAuthModel Organization
        {
            get
            {
                if (IsAuthenticated && _organization == null)
                    _organization = _context.Set<Organization>().Where(a => a.Id == User.OrganizationId).MapToAuthModel().FirstOrDefault();

                return _organization;
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
                    _userId = User.Id;

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

        public ContractorAuthModel Contractor => null;

        public bool HasPermission(params ModuleCode[] moduleCodes)
        {
            return moduleCodes.Any(a => HasPermission(a.ToString()));
        }

        protected override void Clear()
        {
            base.Clear();
            _user = null;
            _modules = null;
            //_parentOrganization = null;
            _organization = null;
            _userId = null;
        }

        public void SelectContractor(long contractorId)
        {
            throw new NotImplementedException();
        }

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
