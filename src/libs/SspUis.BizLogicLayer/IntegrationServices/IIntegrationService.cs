using StatusGeneric;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.IntegrationServices;

public interface IIntegrationService : IStatusGeneric
{
    Task CheckAllIntegrations();
}
