using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Appeal;
using SspUis.BizLogicLayer.AppealDescriptionServices;
using SspUis.BizLogicLayer.AppealTypeArriveServices;
using SspUis.BizLogicLayer.Hrm.StaffingServices;
using SspUis.BizLogicLayer.ManualServices;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.Repositories.Appeal;
using WEBASE.AspNet;
using WEBASE.Models;
using WEBASE.Storage;

namespace SspUis.My.WebApi.Controllers;

[ApiController]
[Route("appeal/[controller]/[action]")]
public class ChamberAppealApplicationController : WebaseController
{
    private readonly IAppealApplicationService _service;
    private readonly IStaffingService _staffingServiceservice;
    private readonly IManualService _manualService;

    public ChamberAppealApplicationController(IAppealApplicationService service, IStaffingService staffingServiceservice, IManualService manualService)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
        this._staffingServiceservice = staffingServiceservice;
        this._manualService = manualService;
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AppealApplicationDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AppealApplicationDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(AppealApplicationDto), 200)]
    public IActionResult GetFromDocnumber(string docNumber)
    {
        var dto = _service.Get(docNumber);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public async Task<IActionResult> Create(CreateAppealApplicationDlDto dto)
    {
        if (ModelState.IsValid)
        {
            dto.IsCreatedByChamber = true;
            HaveId<long> result = await _service.Create(dto);

            if (_service.IsValid)
                return Ok(result);
            //return BadRequest("Hizmat vaqtincha ish holatida emas.");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    [AllowAnonymous]
    [HttpPost("{id}")]
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
    [ProducesResponseType(200)]
    public async Task<IActionResult> Update(UpdateAppealApplicationDlDto dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Update(dto);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [AllowAnonymous]
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
    [AllowAnonymous]
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PersonDto), 200)]
    public async Task<IActionResult> GetByPassportData(
        [FromQuery] SspUis.Integration.DigitizationCenter.Models.GSP.GSPNewApiRequestDto dto,
        [FromServices] IPersonService service)
    {
        if (ModelState.IsValid)
        {
            var person = await service.GetByPassportDataFromDigital(dto);

            if (service.IsValid)
                return Ok(person);

            service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }


    [HttpGet]
    [AllowAnonymous]
    public SelectList<int> AppealFormatTypeSelectList()
    {
        return _manualService.AppealFormatTypeSelectList();
    }

    [HttpGet]
    [AllowAnonymous]
    public SelectList<int> AppealTypeSelectList()
    {
        return _manualService.AppealTypeSelectList();
    }
    [HttpGet]
    [AllowAnonymous]
    public SelectList<int> AppealDescriptionSelectList(
        [FromServices] IAppealDescriptionService appealDescriptionService,
        bool hasParent)
    {
        return appealDescriptionService.AsSelectList(hasParent);
    }
    [HttpGet]
    [AllowAnonymous]
    public SelectList<int> AppealTypeArriveSelectList(
        [FromServices] IAppealTypeArriveService appealTypeArriveService,
        bool hasParent)
    {
        return appealTypeArriveService.AsSelectList();
    }
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<StaffingDtoForChamber>), 200)]
    public IActionResult GetPositionsForChamber(int? langId)
    {
        if (_service.IsValid)
            return Ok(_staffingServiceservice.GetPositionsForChamber(langId));

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }
}