using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Partner
{
    public interface IPrtnReportsDashboardService : IStatusGeneric
    {
        Task<PrtnReportsDashTotalStatisticsDto> GetPrtnStatisticsList(DashboardFilterOption options);
        CustomsPrivilegeDto GetPrtnCustomsPrivilegeList(DashboardFilterOption options);
        List<PrtnEntepreneurialDashDto> GetPrtnEntrepreneurFoundationList(DashboardFilterOption options);
        CreditsDto GetReportsCreditsList(DashboardFilterOption options);
        List<StateAssetDashDto> GetReportsStateAssetsList(DashboardFilterOption options);
        List<PrtnTaxPrivilegeRepotsDashDto> GetPrtnTaxPrivilegeList(DashboardFilterOption options);
        List<PrtnReportsDashRateDto> GetPrtnRateList(RegionRateFilterOption options);
    }
}
