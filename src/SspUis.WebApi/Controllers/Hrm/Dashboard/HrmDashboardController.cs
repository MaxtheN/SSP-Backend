using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm;
using SspUis.Core.Security;

namespace SspUis.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("HrmDashboard/[action]")]
    public class HrmDashboardController : WebaseController
    {
        private readonly IHrmDashboardService _service;
        public HrmDashboardController(IHrmDashboardService service)
            => _service = service;
        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeByRegionList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeByRegionList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeGenderList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeGenderList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeAgeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeAgeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeHigherEduTypeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeHigherEduTypeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeAcademicDegreeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeAcademicDegreeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeLegalEducationList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeLegalEducationList(options));
        }  
        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeExperienceList(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeExperienceList(options));
        }
        [HttpPost]
        //[Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeBithDate(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeBithDate(options));
        }
        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeDateBeforeRecall(HrmDashFilterOption options)
        {
            return Ok(_service.GetEmployeeDateBeforeRecall(options));
        }
        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        [ProducesResponseType(typeof(List<StaffingSinglePageReportDto>), 200)]
        public IActionResult GetStaffingSingleReport(StaffingSinglePageReportDtoFilter filter)
        {
            return Ok(_service.GetStaffingSingleReport(filter));
        }

        //shu yidan boshlandi
        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetEmployeeByWithoutReasonList(HrmDashFilterOption options)
        {
            return Ok(_service.GetWithoutReasonEmployeeList(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetCameToWorkEmployeeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetCameToWorkEmployeeListAsync(options));
        }


        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetLeaveFromWorkEmployeeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetLeaveFromWorkEmployeeListAsync(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetLateToWorkEmployeeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetLateToWorkEmployeeListAsync(options));
        }

        [HttpPost]
        [Authorize(ModuleCode.HrmDashboardView)]
        public IActionResult GetDidNotComeToWorkEmployeeList(HrmDashFilterOption options)
        {
            return Ok(_service.GetDidNotComeToWorkEmployeeListAsync(options));
        }

    }
}
