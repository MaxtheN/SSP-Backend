using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Edoc.Configs;
using SspUis.Integration.Edoc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Edoc.Extensions
{
    public static class ServiceExtentions
    {
        public static void ConfigureEdocRegistrationServices(this IServiceCollection services,
                                                          EdocRegistrationConfig config)
        {
            services.AddSingleton(config); 
            services.AddScoped<IEdocRegistrateService, EdocRegistrateService>();
        }
    }
}
