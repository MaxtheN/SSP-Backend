using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.Srv;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Hrm;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.WebApi.Controllers.Hrm;

[ApiController]
[Authorize]
[Route("hrm/[controller]/[action]")]
public class EmployeeLeaveOrderController : WebaseController
{
    private readonly IEmployeeLeaveOrderService _service;

    public EmployeeLeaveOrderController(IEmployeeLeaveOrderService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    //[HttpPost]
    //[Authorize(ModuleCode.EmployeeLeaveOrderViewAll, ModuleCode.EmployeeLeaveOrderView)]
    //public PagedResult<EmployeeLeaveOrderListDto> GetListAll([FromBody] EmployeeLeaveOrderSortFilter dto)
    //{
    //    return _service.GetListAll(dto);
    //}
    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderView)]
    public PagedResult<EmployeeLeaveOrderListDto> GetList([FromBody] EmployeeLeaveOrderSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }
    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderSignerView)]
    public PagedResult<EmployeeLeaveOrderListDto> GetListForSigner([FromBody] EmployeeLeaveOrderSortFilterOptions dto)
    {
        return _service.GetListForSigner(dto);
    }
    //[HttpPost]
    //[Authorize(ModuleCode.EmployeeLeaveOrderView)]
    //public PagedResult<UnpaidEmployeeLeaveOrderListDto> GetUnpaidList([FromBody] UnpaidEmployeeLeaveOrderSortFilterPageOptions dto)
    //{
    //    return _service.GetUnPaidList(dto);
    //}
    [HttpGet]
    [Authorize(ModuleCode.EmployeeLeaveOrderView)]
    [ProducesResponseType(typeof(EmployeeLeaveOrderDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.EmployeeLeaveOrderView, ModuleCode.EmployeeLeaveOrderSignerView, ModuleCode.SignerView)]
    [ProducesResponseType(typeof(EmployeeLeaveOrderDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(int? employeeId)
    {
        return Ok(_service.AsSelectList(employeeId));
    }
    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetTableAsSelectList(long employeeLeaveOrderId, long? employeeId)
    {
        return Ok(_service.GetTableAsSelectList(employeeLeaveOrderId, employeeId));
    }
    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderCreate, ModuleCode.AllEmployeeLeaveOrderCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateEmployeeLeaveOrderDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<long> result = _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateEmployeeLeaveOrderDlDto dto)
    {
        if(ModelState.IsValid)
        {
            _service.Update(dto);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни ишдан бўшатиш тўғрисида хабарнома ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusEmployeeLeaveOrderDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.Accept(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни ишдан бўшатиш тўғрисида хабарнома ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusEmployeeLeaveOrderDto dTo)
    {
        if (ModelState.IsValid)
        {
            _service.Cancel(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost("{id}")]
    [Authorize(ModuleCode.EmployeeLeaveOrderDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(long id)
    {
        if(ModelState.IsValid)
        {
            _service.Delete(id);

            if(_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.EmployeeLeaveOrderSign)]
    public async Task<IActionResult> Sign(SignStatusEmployeeLeaveOrdeDto dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Sign(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2, string? lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2, lang);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [Authorize(ModuleCode.EmployeeLeaveOrderCreate)]
    [ProducesResponseType(200)]
    public IActionResult GetCalculatedDays(
           int employeeManageId,
           DateTime startDate,
           DateTime endDate)
    {
        if (ModelState.IsValid)
        {
            int days = _service.GetCalculatedDays(employeeManageId: employeeManageId,
                                                  startDate: startDate,
                                                  endDate: endDate);

            if (_service.IsValid)
                return Ok(days);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadTemplate()
    {
        if (ModelState.IsValid)
        {
            var bytes = _service.GetWordTemplate();

            if (_service.IsValid)
                return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DownloadTemp.docx");

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
    [Authorize(ModuleCode.EmployeeLeaveOrderSign)]
    [HttpPost]
    public async ValueTask<IActionResult> WebImzoSign(WebImzoSignedFilter filter)
    {
        if (ModelState.IsValid)
        {
            var url = await _service.SendUrl(filter.Id);

            if (_service.IsValid)
                return Ok(new { Url = url });

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
