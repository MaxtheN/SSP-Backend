using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using SspUis.DataLayer.EfCode;
using SspUis.DataLayer.PgSql.EfCode;
using SspUis.My.WebApi;
using WEBASE.EF;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DbServiceExtentions
    {
        public static void ConfigureDbServices(this IServiceCollection services)
        {
            //if (AppSettings.Instance.System.IsTest)
            //{
            //    var ssh = AppSettings.Instance.Database.SShConfigs;

            //    var client = new SshClient(ssh.Host, ssh.UserName, ssh.Password);
            //    client.Connect();
            //    var portFwd = new ForwardedPortLocal("localhost", (uint)5000, ssh.Host, uint.Parse(ssh.Port));
            //    portFwd.Start();
            //}
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<EfCoreContext, PgSqlContext>(options => options.UseNpgsql(AppSettings.Instance.Database.PgSql.ConnectionString));
            services.AddScoped<BaseDbContext>(x => x.GetService<EfCoreContext>());
        }
    }
}
