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
[Route("hrm/[controller]/[action]")]
public class ChastisementController : WebaseController
{
    private readonly IChastisementService _service;

    public ChastisementController(IChastisementService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    //[HttpPost]
    //[Authorize(ModuleCode.ChastisementViewAll, ModuleCode.ChastisementView)]
    //public PagedResult<ChastisementListDto> GetListAll([FromBody] ChastisementSortFilter dto)
    //{
    //    return _service.GetListAll(dto);
    //}
    [HttpPost]
    [Authorize(ModuleCode.ChastisementView)]
    public PagedResult<ChastisementListDto> GetList([FromBody] ChastisementSortFilterOptions dto)
    {
        return _service.GetList(dto);
    }
    [HttpPost]
    [Authorize(ModuleCode.ChastisementSignerView)]
    public PagedResult<ChastisementListDto> GetListForSigner([FromBody] ChastisementSortFilterOptions dto)
    {
        return _service.GetListForSigner(dto);
    }
    //[HttpPost]
    //[Authorize(ModuleCode.ChastisementView)]
    //public PagedResult<UnpaidChastisementListDto> GetUnpaidList([FromBody] UnpaidChastisementSortFilterPageOptions dto)
    //{
    //    return _service.GetUnPaidList(dto);
    //}
    [HttpGet]
    [Authorize(ModuleCode.ChastisementView)]
    [ProducesResponseType(typeof(ChastisementDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.ChastisementView, ModuleCode.ChastisementSignerView, ModuleCode.SignerView)]
    [ProducesResponseType(typeof(ChastisementDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(int? employeeId)
    {
        return Ok(_service.AsSelectList(employeeId));
    }
    //[HttpGet]
    //[ProducesResponseType(typeof(SelectList<long>), 200)]
    //public IActionResult GetTableAsSelectList(long ChastisementId, long? employeeId)
    //{
    //    return Ok(_service.GetTableAsSelectList(ChastisementId, employeeId));
    //}
    [HttpPost]
    [Authorize(ModuleCode.ChastisementCreate, ModuleCode.AllChastisementCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async ValueTask<IActionResult> Create(CreateChastisementDlDto dto)
    {
        if(ModelState.IsValid)
        {
            HaveId<long> result = await _service.Create(dto);

            if(_service.IsValid)
                return Ok(result);

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.ChastisementEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateChastisementDlDto dto)
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
    [Authorize(ModuleCode.ChastisementAccept)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни ишдан бўшатиш тўғрисида хабарнома ҳужжатини тасдиқлаш")]
    public IActionResult Accept(UpdateStatusChastisementDto dTo)
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
    [ProducesResponseType(200)]
    public IActionResult DownloadByIdFile(Guid fileId)
    {
        if (ModelState.IsValid)
        {
            StorageFile file = _service.DownloadByIdFile(fileId);

            if (_service.IsValid)
                return File(file.GetStream(), "application/pdf");

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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadFile(Guid? id2, Guid? fileId)
    {
        if (ModelState.IsValid)
        {
            var bytes = _service.DownloadFile(id2, fileId);

            return Ok();
            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.ChastisementCancel)]
    [ProducesResponseType(200)]
    //[UserAction("Ходимни ишдан бўшатиш тўғрисида хабарнома ҳужжатини бекор қилиш")]
    public IActionResult Cancel(UpdateStatusChastisementDto dTo)
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
    [Authorize(ModuleCode.ChastisementDelete)]
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
    [Authorize(ModuleCode.ChastisementSign)]
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DownloadPdf(Guid id2)
    {
        if (ModelState.IsValid)
        {
            var bytes = await _service.DownloadPdf(id2);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [HttpPost]
    [Authorize(ModuleCode.ChastisementSign)]
    public async Task<IActionResult> Sign(SignStatusChastisementDto dto)
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
    //[HttpGet]
    //[Authorize(ModuleCode.ChastisementCreate)]
    //[ProducesResponseType(200)]
    //public IActionResult GetCalculatedDays(
    //       int employeeManageId,
    //       DateTime startDate,
    //       DateTime endDate)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        int days = _service.GetCalculatedDays(employeeManageId: employeeManageId,
    //                                              startDate: startDate,
    //                                              endDate: endDate);

    //        if (_service.IsValid)
    //            return Ok(days);

    //        _service.CopyErrorsToModelState(ModelState);
    //    }

    //    return ValidationProblem(ModelState);
    //}
}
