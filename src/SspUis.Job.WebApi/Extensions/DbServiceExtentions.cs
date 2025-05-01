using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.PgSql.EfCode;
using SspUis.Job.WebApi;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbServiceExtentions
    {
        public static void ConfigureDbServices(this IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<EfCoreContext, PgSqlContext>(options =>
            {
                options.UseNpgsql(AppSettings.Instance.Database.PgSql.ConnectionString);
#if DEBUG
                options.EnableSensitiveDataLogging(true);
#endif
            });
            services.AddScoped<BaseDbContext>(x => x.GetService<EfCoreContext>());
        }
    }
}
