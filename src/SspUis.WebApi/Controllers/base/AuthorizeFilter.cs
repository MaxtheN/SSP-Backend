using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBASE.Security;

namespace SspUis.WebApi
{
    public class AuthorizeFilter : Attribute, IAuthorizationFilter
    {
        protected string[] ModuleCodes { get; }

        public AuthorizeFilter(string[] moduleCodes)
        {
            ModuleCodes = moduleCodes;
        }

        public virtual void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(a => a is AllowAnonymousAttribute))
                return;

            var authService = (IAuthService)context.HttpContext.RequestServices.GetService(typeof(IAuthService));

            if (!authService.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }
            else if ((ModuleCodes?.Any()).GetValueOrDefault())
            {
                if (!ModuleCodes.Any(moduleCode => authService.HasPermission(moduleCode)))
                    context.Result = new StatusCodeResult(403);
            }

        }
    }
}
