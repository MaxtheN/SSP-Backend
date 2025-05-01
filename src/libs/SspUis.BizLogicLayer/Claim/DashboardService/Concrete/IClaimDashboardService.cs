using StatusGeneric;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Claim;

public interface IClaimDashboardService : IStatusGeneric
{
    ClaimDashTotalStatisticsDto GetClaimStatisticList(ClaimDashFilterOption options);
    List<ClaimApplicationTypeDto> GetClaimApplicationList(ClaimDashFilterOption options);
    List<ClaimDashDocsDto> GetApplicationForCourtList(ClaimDashFilterOption options);
    List<MediatonResultTypeDto> GetMediationResultList(ClaimDashFilterOption options);
    List<ApplicationRate> GetApplicationRateList(ClaimDashRateFilterOption options);
    List<ContractorTimeLineRate> GetContractorsRateList(ClaimContractorTimeLineOption options);
}
