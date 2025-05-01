using SspUis.Integration.OnlineMahalla;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtentions
    {
        public static void ConfigureOnlineMahallaServices(this IServiceCollection services,
                                                          OnlineMahallaConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IOnlineMahallaService,OnlineMahallaService>();
        }
    }
}
