using StatusGeneric;
using System.Collections.Generic;
namespace SspUis.BizLogicLayer.Memship
{
    public interface IMemshipDashboardService : IStatusGeneric
    {
        List<MemshipDashDocsDto> GetMemshipApplicationList(MemshipDashFilterOption options);
        List<MemshipDashDocsDto> GetMemshipCertificateList(MemshipDashFilterOption options);
        List<MemshipDashDocsDto> GetMemshipContractList(MemshipDashFilterOption options);
        List<MemshipContractRate> GetMemshipContractRateList(MemshipDashRateFilterOption options);
        List<MemshipContractTypeDto> GetMemshipContractTypeList(MemshipDashFilterOption options);
        MemshipDashTotalStatisticsDto GetMemshipStatisticList(MemshipDashFilterOption options);
    }
}
