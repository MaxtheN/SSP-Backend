using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Sud.Configs;
using SspUis.Integration.Sud.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Integration.Sud.Extensions;

public static class SudExtensions
{
    public static void ConfigureSudServices(this IServiceCollection services, SudConfig config)
    {
        services.AddSingleton(config);
        services.AddScoped<ISudService, SudService>();
        services.AddScoped<ISudLoginService, SudLoginService>();
    }
}

