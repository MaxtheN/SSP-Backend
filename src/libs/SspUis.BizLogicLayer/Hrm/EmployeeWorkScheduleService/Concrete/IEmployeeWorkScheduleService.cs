using StatusGeneric;
namespace SspUis.BizLogicLayer.Hrm;

public interface IEmployeeWorkScheduleService : IStatusGeneric
{
    EmployeeWorkScheduleFromSSPDto GetWorkYearFromEmpManage(EmployeeWorkScheduleFromSSPFilterOption option);
    EmployeeWorkScheduleFromMehnatDto GetWorkYearFromMehnat(EmployeeWorkScheduleFromMehnatFilterOption option);
    EmployeeWorkScheduleDto GetTotalWorkYears(EmployeeWorkScheduleFilterOption option);
}