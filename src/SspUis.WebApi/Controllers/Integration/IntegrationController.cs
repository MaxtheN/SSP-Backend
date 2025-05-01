using Microsoft.AspNetCore.Mvc;
using SspUis.BizLogicLayer.IntegrationServices;
using SspUis.BizLogicLayer.IntegrationServices.Bojxona;
using SspUis.BizLogicLayer.IntegrationServices.Finance.Concrete;
using WEBASE.AspNet;
using WEBASE.AspNet.Security;
using WEBASE.Models;

namespace SspUis.WebApi.Controllers;

[ApiController]
[Route("integration/[action]")]
public class IntegrationController : WebaseController
{
    private readonly IIntegrationService _service;
    private readonly IFinanceIntegrationService _financeService;

    public IntegrationController(IIntegrationService integrationService, IFinanceIntegrationService financeService
    )
    {

    
        _service = integrationService;
        _financeService = financeService; }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CheckAllIntegrations()
    {
        await _service.CheckAllIntegrations();

        if (_service.IsValid)
            return Ok();

        _service.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult GetFinancePaymentList([FromBody] FinPaymentDataSortFilterPageOptions dto)
    {
        var result  = _financeService.GetFinancePaymentDataList(dto);

        if (_financeService.IsValid)
            return Ok(result);

        _financeService.CopyErrorsToModelState(ModelState);

        return ValidationProblem(ModelState);
    }

    [BasicAuth("finance_payment")]
    [HttpPost]
    [ProducesResponseType(200)]
    public IActionResult PostFinancePaymentData(FinPaymentDataDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _financeService.CreateFinancePymentData(dto);

            if (_financeService.IsValid)
                return Ok(result);

            _financeService.CopyErrorsToModelState(ModelState);
        }

        return ValidationProblem(ModelState);
    }
    

}
