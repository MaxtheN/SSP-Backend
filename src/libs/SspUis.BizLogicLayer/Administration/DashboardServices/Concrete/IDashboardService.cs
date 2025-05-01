namespace SspUis.BizLogicLayer.DashboardServices
{
    public interface IDashboardService
    {
        DashboardDataDto GetDashboardData();
        LandingPageDataDto GetLandingPageData();
    }
}