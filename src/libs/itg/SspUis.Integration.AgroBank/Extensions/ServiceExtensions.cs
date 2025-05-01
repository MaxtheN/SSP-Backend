using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.AgroBank.Configs;
using SspUis.Integration.AgroBank.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.AgroBank.Extensions;

public static class ServiceExtentions
{
    public static void ConfigureIntegrationAgroBankServices(this IServiceCollection services, AgroBankConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<IAgroBankService, AgroBankService>();
       
    }
}
