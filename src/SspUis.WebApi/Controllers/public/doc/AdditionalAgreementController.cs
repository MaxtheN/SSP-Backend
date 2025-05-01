using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer;
using SspUis.BizLogicLayer.Hrm.AdditionalAgreementService;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class AdditionalAgreementController : WebaseController
{
    private IAdditionalAgreementService _service;

    public AdditionalAgreementController(IAdditionalAgreementService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        _service = service;
    }
    [HttpPost]
    [Authorize(ModuleCode.AdditionalAgreementViewAll)]
    public PagedResult<AdditionalAgreementListDto> GetList([FromBody] AdditionalAgreementSortFilterOption dto)
    {
        return _service.GetList(dto);
    }

    [HttpGet("{memshipContractId}")]
    [Authorize(ModuleCode.AdditionalAgreementView)]
    [ProducesResponseType(typeof(MemshipAdditionalAgreementDto), 200)]
    public IActionResult GetByMemshipContractId(int memshipContractId)
    {
        var dto = _service.GetByMemshipContractId(memshipContractId);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [Authorize(ModuleCode.AdditionalAgreementView)]
    [ProducesResponseType(typeof(AdditionalAgreementDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }

    [HttpGet("{id}")]
    [Authorize(ModuleCode.AdditionalAgreementView)]
    [ProducesResponseType(typeof(AdditionalAgreementDto), 200)]
    public IActionResult Get(long id)
    {
        AdditionalAgreementDto dto = _service.Get(id);

        if (_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpGet]
    [ProducesResponseType(typeof(SelectList<int>), 200)]
    public IActionResult GetAsSelectList()
    {
        return Ok(_service.AsSelectList());
    }

    [HttpPost]
    [Authorize(ModuleCode.AdditionalAgreementCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateAdditionalAgreementDlDto dto)
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
    [Authorize(ModuleCode.MemshipContractSign)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Sign(SignStatusAdditionalAgreementDto dto)
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
    [HttpPost]
    [Authorize(ModuleCode.MemshipContractSign)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Reject(RejectStatusAdditionalAgreementDto dto)
    {
        if (ModelState.IsValid)
        {
            await _service.Reject(dto);

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

    [HttpPost]
    [Authorize(ModuleCode.AdditionalAgreementEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateAdditionalAgreementDlDto dto)
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

    [HttpPost("{id}")]
    [Authorize(ModuleCode.AdditionalAgreementDelete)]
    [ProducesResponseType(200)]
    public IActionResult Delete(int id)
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
}
