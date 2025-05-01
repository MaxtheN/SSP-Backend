using SspUis.DataLayer.EfCode;
using SspUis.BizLogicLayer.CountryServices;
using GenericServices.Setup;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SspUis.BizLogicLayer.BusinessmanAccountServices;
using SspUis.BizLogicLayer.ClaimApplicationServices;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GenericServiceExtentions
    {
        public static void ConfigureGenericServices(this IServiceCollection services)
        {
            services.GenericServicesSimpleSetup<EfCoreContext>(
                Assembly.GetAssembly(typeof(CountryDto)), //Service layer
                Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
            );

            services.RegisterAssemblyPublicNonGenericClasses(
                Assembly.GetAssembly(typeof(CountryDto)), //Service layer
                Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
            )
            .Where(a => !a.Equals(typeof(BusinessmanAccountService)))
            .Where(a => !a.Equals(typeof(ClaimApplicationService)))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
        }
    }
}
