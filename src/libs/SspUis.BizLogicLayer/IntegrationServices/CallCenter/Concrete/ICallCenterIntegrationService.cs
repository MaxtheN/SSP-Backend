

using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.IntegrationServices;

public interface ICallCenterIntegrationService : IStatusGenericHandler
{
    HaveId<long> CreateCallCenter(int  callCount);
    int GetLastTime();
}
