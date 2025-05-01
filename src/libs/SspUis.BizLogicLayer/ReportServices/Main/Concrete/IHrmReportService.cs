using SspUis.BizLogicLayer.ReportServices.Main;
using StatusGeneric;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices;

public interface IHrmReportService : IStatusGenericHandler
{
    Task<List<HrmEmployeeActivityInfoDto>> GetHrmEmployeeActivityInfoAsync(HrmEmployeeActivityInfoDtoFilter filter);

    Task<Stream> SaveHrmReportAsExcelAsync(HrmEmployeeActivityInfoDtoFilter filter);
}
