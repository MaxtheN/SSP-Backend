using SspUis.BizLogicLayer.Arbitration;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Arbitration;

public interface IArbitrationDashboardService : IStatusGeneric
{
    ArbitrationTypeCountDto GetAllArbitrationTypeAmount(ArbitrationDashFilterOptions options);
    List<ArbitrationRateDto> GetAllArbitrationRateDto(ArbitrationRateFilterOptions filter);
    List<DeadlineNearArbitrationApplicationDto> GetAllDeadlineNearApplication(ArbitrationApplicationDtoFilter filter);
}
