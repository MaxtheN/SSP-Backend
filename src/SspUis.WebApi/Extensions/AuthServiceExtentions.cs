using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.WebApi;
using SspUis.WebApi.Security;
//using WEBASE;

namespace Microsoft.Extensions.DependencyInjection;

public static class CultureServiceExtentions
{
    public static void ConfigureAuthServices(this IServiceCollection services)
    {
        if (string.IsNullOrEmpty(AppSettings.Instance.TestByUserName))
            services.AddScoped<IAuthService, AuthService>();
        else
            services.AddScoped<IAuthService, TestByUserNameAuthService>(p =>
                new TestByUserNameAuthService(AppSettings.Instance.TestByUserName, 
                p.GetRequiredService<IHttpContextAccessor>(), 
                p.GetRequiredService<DbContext>(), 
                AppSettings.Instance.Jwt));

        services.AddScoped<WEBASE.Security.IAuthService>(x => x.GetService<IAuthService>());
    }
}
