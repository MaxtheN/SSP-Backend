using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SspUis.Integration.OneId;

public static class ServiceExtensions
{
    public static void ConfigureOneIdServices(this IServiceCollection services, OneIdConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<IOneIdService, OneIdService>();
    }
}
