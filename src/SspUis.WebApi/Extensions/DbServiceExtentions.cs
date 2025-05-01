using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.PgSql.EfCode;
using SspUis.WebApi;
using WEBASE.EF;

namespace Microsoft.Extensions.DependencyInjection;

public static class DbServiceExtentions
{
    public static void ConfigureDbServices(this IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddDbContext<EfCoreContext, PgSqlContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(AppSettings.Instance.Database.PgSql.ConnectionString)
                   .AddInterceptors(new HintInterceptor());
#if DEBUG
            options.EnableSensitiveDataLogging(true);
#endif
        });

        services.AddScoped<BaseDbContext>(x => x.GetService<EfCoreContext>()!);
    }
}
