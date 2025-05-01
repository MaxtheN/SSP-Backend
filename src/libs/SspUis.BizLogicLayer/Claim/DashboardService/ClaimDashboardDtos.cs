using SspUis.Core;

namespace SspUis.BizLogicLayer.Claim;

public class ClaimDashTotalStatisticsDto
{
    public int TotalMediationPlanCount { get; set; }
    public int TotalClaimApplicationsCount { get; set; }
    public int TotalNewApplicationsCount { get; set; }
    public int TotalApplicationsCount { get; set; }
    public int TotalMediationsCount { get; set; }
}

public class ClaimApplicationTypeDto
{
    public int ClaimApplicationTypeId { get; set; }
    public string ClaimApplicationType { get; set; }
    public long InTermApplicationsCount { get; set; }
    public long ExpiredApplicationsCount { get; set; }
    public long NonClosedApplicationsCount { get; set;}
}

public class MediatonResultTypeDto : ClaimDashFilterOption
{
    public int MediationResultTypeId { get; set; }
    public string MediationResultType { get; set; }
    public long ApplicationsCount { get; set; }
}

public class ClaimDashDocsDto : ClaimDashFilterOption
{
    public long ApplicationsCount { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
}

public class ApplicationRate : ClaimDashFilterOption
{
    public string RegionOrderCode { get; set; }
    public string DistrictOrderCode { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
}

public class ContractorTimeLineRate : ClaimDashFilterOption
{
    public long ApplicationsCount { get; set; }
    public string Contractor { get; set; }
    public long ContractorId { get; set; }
}

public static class ConstStatusParams
{
    public static int[] ClaimApplicationStatuses { get; private set; } = new[]
    {
        StatusIdConst.REJECTED, StatusIdConst.SENT, StatusIdConst.ACCEPTED
    };

    public static int[] ApplicationForCourtStatuses { get; private set; } = new[]
    {
        StatusIdConst.NOT_ACCEPTED, StatusIdConst.SENT, StatusIdConst.ACCEPTED
    };

    public static int[] ResultStatuses { get; private set; } = new[]
    {
        MediationResultIdConst.AGREEMANT_REACHED, MediationResultIdConst.NO_AGREEMANT_REACHED, MediationResultIdConst.GIVEN_TIME
    };
}