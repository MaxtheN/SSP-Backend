using SspUis.Core.Security;
using SspUis.My.WebApi;
using SspUis.My.WebApi.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.EF;
using SspUis.WebApi.Security;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CultureServiceExtentions
    {
        public static void ConfigureAuthServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IIntegrationAuthService, IntegrationAuthService>();

            services.AddScoped<WEBASE.Security.IAuthService>(x => x.GetRequiredService<IAuthService>());
        }
    }
}
