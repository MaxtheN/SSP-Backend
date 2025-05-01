using GenericServices.Setup;
using NetCore.AutoRegisterDi;
using SspUis.BizLogicLayer.BusinessmanAccountServices;
using SspUis.BizLogicLayer.ClaimApplicationServices;
using SspUis.BizLogicLayer.CountryServices;
using SspUis.DataLayer.EfCode;
using SspUis.Job.BizLogicLayer.Services;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class GenericServiceExtentions
{
    public static void ConfigureGenericServices(this IServiceCollection services)
    {
        services.GenericServicesSimpleSetup<EfCoreContext>(
            Assembly.GetAssembly(typeof(CountryDto)), //Service layer
            Assembly.GetAssembly(typeof(ApplicationJobService)), // Job service layer
            Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
        );

        services.RegisterAssemblyPublicNonGenericClasses(
            Assembly.GetAssembly(typeof(CountryDto)), //Service layer
            Assembly.GetAssembly(typeof(ApplicationJobService)), // Job service layer
            Assembly.GetAssembly(typeof(EfCoreContext)) //Data layer
        )
        .Where(a => !a.Equals(typeof(BusinessmanAccountService)))
        .Where(a => !a.Equals(typeof(StateAssetApplicationJobService)))
        .Where(a => !a.Equals(typeof(ClaimApplicationService)))
        .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    }
}
