using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SspUis.Core.Security;

namespace SspUis.My.WebApi;

public class IntegrationAuthorizeFilter : Attribute, IAuthorizationFilter
{
    protected string[] UserNames { get; }

    public IntegrationAuthorizeFilter(string[] userNames)
    {
        UserNames = userNames;
    }

    public virtual void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.Filters.Any(a => a is AllowAnonymousAttribute))
            return;

        var authService = (IIntegrationAuthService)context.HttpContext.RequestServices.GetService(typeof(IIntegrationAuthService));

        if (!authService.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
        }
        else if (UserNames?.Any() == true)
        {
            var authUserName = authService.UserName;
            if (!UserNames.Any(userName => userName.ToLower() == authUserName.ToLower()))
                context.Result = new StatusCodeResult(403);
        }
    }
}