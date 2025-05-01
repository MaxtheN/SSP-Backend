using SspUis.BizLogicLayer.Srv.Doc;
using SspUis.BizLogicLayer.Srv.Doc.SrvApplicationService.DashboardService;
using StatusGeneric;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer;

public interface ISrvApplicationDashService : IStatusGeneric
{
    SrvApplicationTypeDashDto GetSrvApplicationTypeDash(SrvDashboardFilterOption filter);
    SrvContractTypeDashDto GetSrvContractTypeCount(SrvContractTypeDashFilter filter);
    List<SrvApplicationTypeRateDto> GetSrvApplicationRate(SrvApplicationTypeCostFilter filter);
    List<SrvApplicationStatusDto> GetSrvApplicationStatusCount(SrvDashboardFilterOption filter);
    List<SrvContractStatusDto> GetSrvContractStatusCount(SrvDashboardFilterOption filter);
    SrvDeedSumDto GetSrvDeedSum(SrvDashboardFilterOption filter);
    List<SrvDocumentRegionRate> GetSrvDocumentsRegionRate(SrvRegionRateFilterOptions filter);
}