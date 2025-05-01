using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.i18n;

namespace SspUis.Job.WebApi
{
    public class CultureHelper : BaseCultureHelper
    {
        private readonly DbContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CultureHelper(CultureConfig config, DbContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
            : base(config)
        {
            _context = context;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override IEnumerable<CultureModel> GetCultures()
        {
            return _context.Set<Language>().AsEnumerable().Select(a => new CultureModel(a.Id, a.Code, a.FullName));
        }

        protected override CultureModel GetCurrentCulture()
        {
            if (_httpContextAccessor.HttpContext != null 
                && _httpContextAccessor.HttpContext.Request.Query.TryGetValue("__lang", out StringValues lang) && 
                !StringValues.IsNullOrEmpty(lang) && Cultures.Any(a => a.Code == lang))
                return Cultures.First(a => a.Code == lang);
            if (_authService.IsAuthenticated)
                return Cultures.FirstOrDefault(a => a.Code == _authService.User.LanguageCode) ?? DefaultCulture;
            return DefaultCulture;
        }

        protected override CultureModel GetDefaultCulture()
        {
            return Cultures.FirstOrDefault(a => a.Code == "ru");
        }
    }
}
