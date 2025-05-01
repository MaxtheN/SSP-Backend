using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.Claim;
using SspUis.Core.Security;
using SspUis.DataLayer.Repositories.Claim;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class MediationPlanController : WebaseController
{
    private readonly IMediationPlanService _service;

    public MediationPlanController(IMediationPlanService service)
        : base(AppSettings.Instance.ControllerConfig)
    {
        this._service = service;
    }

    [HttpPost]
    [Authorize(ModuleCode.MediationPlanViewAll)]
    public PagedResult<MediationPlanListDto> GetList([FromBody] MediationPlanSortFilterOptions options)
    {
        return _service.GetList(options);
    }

    [HttpGet]
    //[Authorize(ModuleCode.MediationPlanView)]
    [ProducesResponseType(typeof(MediationPlanDto), 200)]
    public IActionResult Get()
    {
        return Ok(_service.Get());
    }
    
    [HttpGet]
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    //[Authorize(ModuleCode.MediationPlanView)]
    [ProducesResponseType(typeof(MediationPlanDto), 200)]
    public IActionResult GetByApplication(int applicationId)
    {
        return Ok(_service.GetByApplication(applicationId));
    }

    [HttpGet("{id}")]
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanView)]
    [ProducesResponseType(typeof(MediationPlanDto), 200)]
    public IActionResult Get(long id)
    {
        var dto = _service.Get(id);

        if(_service.IsValid)
            return Ok(dto);

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    [Authorize(ModuleCode.MediationPlanViewAll)]
    [ProducesResponseType(typeof(SelectList<long>), 200)]
    public IActionResult GetAsSelectList(MediationPlanSortFilterOptions options)
    {
        return Ok(_service.AsSelectList(options));
    }

    [HttpPost]
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanCreate)]
    [ProducesResponseType(typeof(HaveId<long>), 200)]
    public IActionResult Create(CreateMediationPlanDlDto dto)
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
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanEdit)]
    [ProducesResponseType(200)]
    public IActionResult Update(UpdateMediationPlanDlDto dto)
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
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanAccept)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Accept(UpdateStatusMediationPlanDto dTo)
    {
        if (ModelState.IsValid)
        {
            await _service.Accept(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost]
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanCancel)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Cancel(UpdateStatusMediationPlanDto dTo)
    {
        if (ModelState.IsValid)
        {
            await _service.Cancel(dTo);

            if (_service.IsValid)
                return Ok();

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }

    [HttpPost("{id}")]
    //[CheckDocumentLock(TableIdConst.CLAIM__DOC_MEDIATION_PLAN, AppIdConst.ERP)]
    [Authorize(ModuleCode.MediationPlanDelete)]
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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    public IActionResult DownloadPdf(Guid id2, string? lang)
    {
        if (ModelState.IsValid)
        {
            var bytes = _service.DownloadPdf(id2, lang);

            if (_service.IsValid)
                return File(bytes, "application/pdf");

            _service.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
}
