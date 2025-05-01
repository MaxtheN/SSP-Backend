using StatusGeneric;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace SspUis.BizLogicLayer.Hrm
{
    public interface IHrmDashboardService : IStatusGeneric
    {
        List<HrmEmployeeByRegionDto> GetEmployeeByRegionList(HrmDashFilterOption options);
        List<HrmEmployeeDto> GetEmployeeList(HrmDashFilterOption options);
        List<HrmEmployeeGenderDto> GetEmployeeGenderList(HrmDashFilterOption options);
        List<HrmEmployeeAgeDto> GetEmployeeAgeList(HrmDashFilterOption options);
        List<HrmEmployeeHigherEduCount> GetEmployeeHigherEduTypeList(HrmDashFilterOption options);
        List<HrmEmployeeAcademicDegree> GetEmployeeAcademicDegreeList(HrmDashFilterOption options);
        List<HrmEmployeeLegalEducation> GetEmployeeLegalEducationList(HrmDashFilterOption options);
        List<HrmEmployeeExperience> GetEmployeeExperienceList(HrmDashFilterOption options);
        List<HrmEmployeeBirthDay> GetEmployeeBithDate(HrmDashFilterOption options);
        List<StaffingSinglePageReportDto> GetStaffingSingleReport(StaffingSinglePageReportDtoFilter dto);
        List<HrmEmployeeLeaveOrderDto> GetEmployeeDateBeforeRecall(HrmDashFilterOption options);

        List<HrmEmployeeDto> GetWithoutReasonEmployeeList(HrmDashFilterOption options);
        Task<List<HrmEmployeeDto>> GetCameToWorkEmployeeListAsync(HrmDashFilterOption options);
        Task<List<HrmEmployeeDto>> GetLeaveFromWorkEmployeeListAsync(HrmDashFilterOption options);
        Task<List<HrmEmployeeDto>> GetLateToWorkEmployeeListAsync(HrmDashFilterOption options);
        Task<List<HrmEmployeeDto>> GetDidNotComeToWorkEmployeeListAsync(HrmDashFilterOption options);
    }
}
