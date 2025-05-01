using SspUis.My.WebApi;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CorsServiceExtensions
    {
        public static void ConfigureCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
            var corsConfig = configuration.GetSection("Cors").Get<CorsConfig>();

            if (corsConfig == null)
            {
                throw new InvalidOperationException("CORS konfiguratsiyasi topilmadi.");
            }

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });

                options.AddPolicy("AllowedOrigins",
                    builder =>
                    {
                        if (corsConfig.UseCors)
                        {
                            if (corsConfig.AllowedOrigins == null || !corsConfig.AllowedOrigins.Any())
                            {
                                throw new InvalidOperationException("CORS konfiguratsiyasi noto‘g‘ri sozlangan yoki bo‘sh.");
                            }

                            builder.WithOrigins(corsConfig.AllowedOrigins.ToArray())
                                   .AllowAnyMethod()
                                   .AllowAnyHeader()
                                   .AllowCredentials();
                        }
                        else
                        {
                            builder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                        }
                    });
            });
        }
    }

}