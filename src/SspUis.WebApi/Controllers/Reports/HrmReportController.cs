using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.BizLogicLayer.ReportServices.Main;
using SspUis.Core.Security;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class HrmReportController : WebaseController
{
    private IHrmReportService _reportService;

    public HrmReportController(IHrmReportService reportService)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _reportService = reportService;
    }

    [HttpPost]
    [Authorize(ModuleCode.ReportHrmEmployeeActivityInfoView, ModuleCode.ReportHrmEmployeeActivityInfoByRegionView)]
    [ProducesResponseType(typeof(List<HrmEmployeeActivityInfoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHrmEmployeeActivityReport(HrmEmployeeActivityInfoDtoFilter filter)
    {
        return Ok(await _reportService.GetHrmEmployeeActivityInfoAsync(filter)
               .ConfigureAwait(false));
    }

    [HttpPost]
    public async Task<IActionResult> SaveHrmReportAsExcel(HrmEmployeeActivityInfoDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var file = await _reportService.SaveHrmReportAsExcelAsync(filter);
            if (_reportService.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HrmEmployeeActivityReportTemplate.xlsx");

            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}
