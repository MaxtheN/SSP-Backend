using SspUis.Core.Security;
using SspUis.Job.WebApi;
using SspUis.Job.WebApi.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CultureServiceExtentions
    {
        public static void ConfigureAuthServices(this IServiceCollection services)
        {
            if (AppSettings.Instance.TestByUserName.NullOrEmpty())
                services.AddScoped<IAuthService, AuthService>();
            else
                services.AddScoped<IAuthService, TestByUserNameAuthService>(p =>
                    new TestByUserNameAuthService(AppSettings.Instance.TestByUserName, p.GetRequiredService<IHttpContextAccessor>(), p.GetRequiredService<DbContext>(), AppSettings.Instance.Jwt));

            services.AddScoped<WEBASE.Security.IAuthService>(x => x.GetService<IAuthService>());
        }
    }
}
