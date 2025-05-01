using SspUis.Integration.DocxToPdf;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static void ConfigureDocxToPdfServices(this IServiceCollection services, DocxToPdfConfig config)
        {
            services.AddSingleton(config);
            services.AddScoped<IConvertService, ConvertService>();
        }
    }
}
