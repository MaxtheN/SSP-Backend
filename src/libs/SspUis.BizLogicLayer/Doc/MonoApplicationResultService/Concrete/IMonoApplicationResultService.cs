using StatusGeneric;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Doc
{
    public interface IMonoApplicationResultService : IStatusGeneric
    {
        List<MonoApplicationBandlikResultDto> GetList();
        MonoApplicationBandlikResultDto GetAppId(long  appId);   
    }
}