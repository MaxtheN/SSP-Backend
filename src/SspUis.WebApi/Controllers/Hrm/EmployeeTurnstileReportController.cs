using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Hrm.Info.EmployeeTurnstileReportServices;
using SspUis.Core.Security;
using WEBASE.AspNet;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class EmployeeTurnstileReportController : WebaseController
{
    private readonly IEmployeeTurnstileReportService _reportService;

    public EmployeeTurnstileReportController(IEmployeeTurnstileReportService reportService)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _reportService = reportService;
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeTurnstileReport)]
    public async Task<IActionResult> GetEmployeeTurnstileReport([FromBody] EmployeeTurnstileReportDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = await _reportService.GetEmployeeTurnstileReportAsync(filter)
                .ConfigureAwait(false);

            if (_reportService.IsValid)
                return Ok(result);
            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeTurnstileReport)]
    public IActionResult GetEmployeeTurnstileReportById([FromBody] EmployeeTurnstileReportByIdDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = _reportService.GetEmployeeTurnstileReportById(filter);

            if (_reportService.IsValid)
                return Ok(result);
            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeTurnstileReport)]
    public IActionResult GetEmployeeTurnstileTimeReport([FromBody] EmployeeTurnstileTimeReportDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var result = _reportService.GetEmployeeTurnstileTimeReport(filter);

            if (_reportService.IsValid)
                return Ok(result);
            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
	public async Task<IActionResult> SaveEmployeeTurnstileReportAsExcel(EmployeeTurnstileReportDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var file =  _reportService.SaveEmployeeTurnstileReportAsExcel(filter).Result;

            if (_reportService.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Турникет для сотрудников {filter.OnDate?.ToString("dd.MM.yyyy")}-{filter.EndDate?.ToString("dd.MM.yyyy")}.xlsx");

            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }

    [HttpPost]
    public async Task<IActionResult> SaveEmployeeTurnstileReportById(EmployeeTurnstileReportByIdDtoFilter filter)
    {
        if (ModelState.IsValid)
        {
            var file =  _reportService.SaveEmployeeTurnstileReportById(filter).Result;

            if (_reportService.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Турникет для сотрудника {filter.OnDate?.ToString("dd.MM.yyyy")}-{filter.EndDate?.ToString("dd.MM.yyyy")}.xlsx");

            _reportService.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}
