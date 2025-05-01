using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Integration.Manuals.Services;

namespace WEBASE.Integration.Manuals.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddIntegrationManuals(this IServiceCollection services)
        {
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICitizenshipService, CitizenshipService>();
            services.AddScoped<IDistrictService, DistrictService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<INationalityService, NationalityService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IUOWIntegrationManuals, UOWIntegrationManuals>();
            return services;
        }
    }
}
