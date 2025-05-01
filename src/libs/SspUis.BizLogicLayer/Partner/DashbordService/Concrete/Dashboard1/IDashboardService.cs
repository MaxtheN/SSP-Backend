using System.Collections.Generic;
using StatusGeneric;
namespace SspUis.BizLogicLayer.Partner;

public interface IDashboardService : IStatusGeneric
{
    List<PrtnDocumentsRegionRate> GetPrtnDocumentsRegionRate(RegionRateFilterOption options);
    List<PrtnDocumentsRegionRate> GetPrtnContractsRegionRate(RegionRateFilterOption options);
    List<PrtnApplicationCountDto> GetPrtnApplicationList(DashboardFilterOption options);
    List<PrtnContractCountDto> GetPrtnContractList(DashboardFilterOption options);
    PrtnCertificateCountDto GetPrtnCertificateList(DashboardFilterOption options);
    PrtnStatisticsDto GetPrtnStatisticsList(DashboardFilterOption options);
    List<PrtnContractTypeDto> GetPrtnContractTypeList(DashboardFilterOption options);
}
