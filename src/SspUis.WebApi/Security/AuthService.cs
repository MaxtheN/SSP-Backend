using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.AccountServices;
using SspUis.Core.Security;
using System.Security.Claims;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.AspNet.Security;
using WEBASE.EF;

namespace SspUis.WebApi.Security;

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
                                .FirstOrDefault()!;

                if (_user != null && _user.EmployeeManageId != null)
                {
                    var chastisement = _context.Set<ChastisementTable>()
                        .Any(x =>
                        x.EmployeeManageId == _user.EmployeeManageId &&
                            x.IsBlocked
                        );
                    if (chastisement)
                        _user?.ResolveModules(true);
                    else
                        _user?.ResolveModules();
                }
                else
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
                _oneIdToken = new JwtSecurityTokenHandler().ReadJwtToken(ReadTokenFromRequest()).Claims.FirstOrDefault((Claim a) => a.Type == "one-token")!.Value;
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
                _organization = _context.Set<Organization>().Where(a => a.Id == User.OrganizationId).MapToAuthModel().FirstOrDefault()!;

            return _organization;
        }
    }

    private int? _userId;

    public override string UserName => base.UserName;

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
