using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SspUis.Integration.Stat.Configs;
using SspUis.Integration.Stat.Services;

namespace SspUis.Integration.Stat.Extension
{
	public static class StatExtension
	{
		public static void ConfigureStatServices(this IServiceCollection services, StatConfig config)
		{
			services.AddSingleton(config);
			services.AddScoped<IStatService, StatService>();
			services.AddScoped<IStatLoginService, StatLoginService>();
		}
	}
}
