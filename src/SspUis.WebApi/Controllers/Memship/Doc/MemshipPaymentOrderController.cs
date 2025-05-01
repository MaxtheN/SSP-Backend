using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Memship;
using SspUis.BizLogicLayer.ReportServices;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("dualedu/[controller]/[action]")]
public class MemshipPaymentOrderController : WebaseController
{
    private IMemshipPaymentOrderService _service;

    public MemshipPaymentOrderController(IMemshipPaymentOrderService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.MemshipPaymentOrderView, ModuleCode.ServicePaymentOrderView, ModuleCode.ServicePaymentOrderViewAll)]
    [ProducesResponseType(typeof(PagedResult<MemshipPaymentOrderListDto>), 200)]
    public IActionResult GetList([FromBody] MemshipPaymentOrderSortFilterOption dto)
    {
        if (_service.IsValid)
            return Ok(_service.GetList(dto));

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [Authorize(ModuleCode.MemshipPaymentOrderView, ModuleCode.ServicePaymentOrderView)]
    [ProducesResponseType(typeof(MemshipPaymentOrderDto), 200)]
    public IActionResult Get()
    {
        if (_service.IsValid)
            return Ok(_service.Get());

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.MemshipPaymentOrderView, ModuleCode.ServicePaymentOrderView)]
    [ProducesResponseType(typeof(MemshipPaymentOrderDto), 200)]
    public IActionResult Get(long id)
    {
        MemshipPaymentOrderDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    //[HttpGet()]
    //[ProducesResponseType(typeof(SelectList<int>), 200)]
    //public IActionResult GetAsSelectList(int? instituteId = null)
    //{
    //    return Ok(_service.AsSelectList(instituteId));
    //}

    [HttpPost]
    [Authorize(ModuleCode.MemshipPaymentOrderCreate, ModuleCode.ServicePaymentOrderCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateMemshipPaymentOrderDlDto dto)
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
    [Authorize(ModuleCode.MemshipPaymentOrderEdit, ModuleCode.ServicePaymentOrderEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMemshipPaymentOrderDlDto dto)
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
    [ProducesResponseType(200)]
    [Authorize(ModuleCode.MemshipPaymentOrderAccept, ModuleCode.ServicePaymentOrderAccept)]
    public IActionResult Accept(long id)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(id);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    [ProducesResponseType(200)]
    [Authorize(ModuleCode.MemshipPaymentOrderCancel, ModuleCode.ServicePaymentOrderCancel)]
    public IActionResult Cancel(long id, string message)
    {
        if (ModelState.IsValid)
        {
            _service.Cancel(id, message);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost("{id}")]
    [Authorize(ModuleCode.MemshipPaymentOrderDelete, ModuleCode.ServicePaymentOrderDelete)]
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
    [ServiceFilter(typeof(UploadFileAttribute))]
    [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
    public IActionResult UploadFile([FromForm] List<IFormFile> files)
    {
        if (ModelState.IsValid)
        {
            StorageFile[] dto = files.Select(a => new StorageFile(a.FileName, a.OpenReadStream())).ToArray();
            var result = _service.UploadFiles(dto);

            if (_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpGet("{fileId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<IStorageFileInfo>), 200)]
    public IActionResult DownloadFile(Guid fileId, [FromServices] IMimeMappingService mimeMappingService)
    {
        if (ModelState.IsValid)
        {
            StorageFile file = _service.DownloadFile(fileId);

            if (_service.IsValid)
                return File(file.GetStream(), mimeMappingService.Map(file.FileName));

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{fileId}")]
    public IActionResult DeleteFile(Guid fileId)
    {
        if (ModelState.IsValid)
        {
            _service.DeleteFile(fileId);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    public IActionResult SaveAsExcelForGetListMemshipPaymentOrder(MemshipPaymentOrderSortFilterOption filter)
    {
        if (ModelState.IsValid)
        {
            var file = _service.SaveAsExcelPrtnBojxonaContracts(filter);
            if (_service.IsValid)
                return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "To'lovlarHujjati.xlsx");
            _service.CopyErrorsToModelState(ModelState);
        }
        return ValidationProblem(ModelState);
    }
}
