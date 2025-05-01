using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Kpi;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Kpi;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers.Kpi;


[Route("kpi/[controller]/[action]")]
[ApiController]
[Authorize]
public class KpiPlanForEmployeeController : WebaseController
{
    private IKpiPlanForEmployeeService _service;

    public KpiPlanForEmployeeController(IKpiPlanForEmployeeService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeViewAll)]
    public PagedResult<KpiPlanForEmployeeListDto> GetList([FromBody] KpiPlanForEmployeeSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet]
    [Authorize(ModuleCode.KpiPlanForEmployeeView)]
    [ProducesResponseType(typeof(KpiPlanForEmployeeDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }
    [HttpPost]
    [ProducesResponseType(typeof(KpiPlanForEmployeeDto), 200)]
    public IActionResult FillTable(int organizationId)
    {
        return Ok(_service.FillTable(organizationId));
    }

    [HttpPost]
    [ProducesResponseType(typeof(KpiPlanForEmployeeDto), 200)]
    public IActionResult FillTableByDepartment(int organizationId)
    {
        return Ok(_service.FillTableByDepartment(organizationId));
    }

    [HttpPost]
    public IActionResult UploadExcelFile(IFormFile file)
    {
        _service.ReadFromExcelFile(file);
        return Ok();
    }


    [HttpGet("{id}")]
    [Authorize(ModuleCode.KpiPlanForEmployeeView)]
    [ProducesResponseType(typeof(KpiPlanForEmployeeDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateKpiPlanForEmployeeDlDto dto)
    {
        if (ModelState.IsValid)
        {
            HaveId<long> result = _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateKpiPlanForEmployeeDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Update(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeAccept)]
    [ProducesResponseType(200)]
    public IActionResult Accept(UpdateStatusKpiPlanForEmployeeDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.KpiPlanForEmployeeCancel)]
    [ProducesResponseType(200)]
    public IActionResult Cancel(UpdateStatusKpiPlanForEmployeeDlDto dto)
    {
        if (ModelState.IsValid)
        {
            _service.Cancel(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost("{id}")]
    [Authorize(ModuleCode.KpiPlanForEmployeeDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(long id)
    {
        if (ModelState.IsValid)
        {
            _service.Delete(id);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


    [HttpPost]
    [AllowAnonymous]
    public IActionResult DownLoadExcelKPIByRegion()
    {
        if (ModelState.IsValid)
        {
            var file = _service.DownloadExcelTemplate();
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KPI.xlsx");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

}

