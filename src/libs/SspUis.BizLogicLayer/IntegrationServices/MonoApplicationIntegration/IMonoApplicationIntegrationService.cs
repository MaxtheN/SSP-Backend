using SspUis.DataLayer.Repositories;
using StatusGeneric;

namespace SspUis.BizLogicLayer.IntegrationServices
{
    public interface IMonoApplicationIntegrationService : IStatusGeneric
    {
        MonoAppResultDto CreateMonoApplicationBandlik(MonoApplicationBandlikResultDlDto dto);
    }
}