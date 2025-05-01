using SspUis.DataLayer.EfCode;
using SspUis.BizLogicLayer.CountryServices;
using GenericServices.Setup;
using NetCore.AutoRegisterDi;
using System.Reflection;
using SspUis.BizLogicLayer.FileValidationServices;
using SspUis.Job.BizLogicLayer.Services;
using SspUis.BizLogicLayer.BusinessmanAccountServices;

namespace Microsoft.Extensions.DependencyInjection
{
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
            ).Where(a => !a.Equals(typeof(FileValidationService)))
            .Where(a => !a.Equals(typeof(PrtnCertificateJobService)))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
        }
    }
}
